﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.CryptoWatcher.Application.Conflicts;
using CesarBmx.Shared.Application.Exceptions;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Shared.Persistence.Extensions;
using CesarBmx.CryptoWatcher.Application.Requests;
using CesarBmx.CryptoWatcher.Domain.Expressions;
using CesarBmx.CryptoWatcher.Application.Messages;
using CesarBmx.CryptoWatcher.Domain.Builders;
using CesarBmx.CryptoWatcher.Domain.Models;
using CesarBmx.CryptoWatcher.Persistence.Contexts;
using CesarBmx.Shared.Application.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CesarBmx.Shared.Messaging.Ordering.Events;
using Twilio.TwiML.Voice;
using CesarBmx.CryptoWatcher.Application.Responses;
using CesarBmx.CryptoWatcher.Domain.Types;

namespace CesarBmx.CryptoWatcher.Application.Services
{
    public class IndicatorService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly ILogger<IndicatorService> _logger;
        private readonly IMapper _mapper;
        private readonly ActivitySource _activitySource;

        public IndicatorService(
            MainDbContext mainDbContext,
            ILogger<IndicatorService> logger,
            IMapper mapper,
            ActivitySource activitySource)
        {
            _mainDbContext = mainDbContext;
            _logger = logger;
            _mapper = mapper;
            _activitySource = activitySource;
        }

        public async Task<List<IndicatorResponse>> GetUserIndicators(string userId)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetUserIndicators));

            // Get user
            var user = await _mainDbContext.Users.FindAsync(userId);

            // Check if it exists
            if (user == null) throw new NotFoundException(UserMessage.UserNotFound);

            // Get all indicators
            var indicators = await _mainDbContext.Indicators
                .Include(x=> x.Dependencies)
                .ThenInclude(x=>x.Dependency)
                .Where(IndicatorExpression.Filter(null, userId)).ToListAsync();

            // Response
            var response = _mapper.Map<List<Responses.IndicatorResponse>>(indicators);

            // Return
            return response;
        }
        public async Task<IndicatorResponse> GetIndicator(string indicatorId)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetIndicator));

            // Get indicator
            var indicator = await _mainDbContext.Indicators
                .Include(x => x.Dependencies)
                .ThenInclude(x => x.Dependency)
                .FirstOrDefaultAsync(x=> x.IndicatorId == indicatorId);

            // Indicator not found
            if (indicator == null) throw new NotFoundException(IndicatorMessage.IndicatorNotFound);

            // Response
            var response = _mapper.Map<Responses.IndicatorResponse>(indicator);

            // Return
            return response;
        }
        public async Task<IndicatorResponse> AddIndicator(AddIndicatorRequest request)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(AddIndicator));
            span.AddTag("UserId", request.UserId);

            // Get user
            var user = await _mainDbContext.Users.FindAsync(request.UserId);

            // User not found
            if (user == null) throw new NotFoundException(UserMessage.UserNotFound);

            // Get indicator
            var indicator = await _mainDbContext.Indicators
                .Include(x => x.Dependencies)
                .FirstOrDefaultAsync(IndicatorExpression.Unique(request.UserId, request.Abbreviation));

            // Throw ConflictException if it exists
            if (indicator != null) throw new ConflictException(new AddIndicatorConflict(AddIndicatorConflictReason.INDICATOR_ALREADY_EXISTS, IndicatorMessage.IndicatorWithSameIdAlreadyExists));

            // Get dependencies
            var dependencies = await GetIndicators(request.Dependencies);

            // Build dependency level
            var dependencyLevel = IndicatorBuilder.BuildDependencyLevel(dependencies);

            // Build indicator dependencies
            var indicatorDependencies = IndicatorDependencyBuilder.BuildIndicatorDependencies(request.IndicatorId, dependencies);

            // Now
            var now = DateTime.UtcNow.StripSeconds();

            // Create
            indicator = new Indicator(
                request.UserId,
                request.Abbreviation,
                request.Name,
                request.Description,
                request.Formula,
                indicatorDependencies,
                dependencyLevel,
                now);

            // Add
            _mainDbContext.Indicators.Add(indicator);

            // Log Id
            var logId = Guid.NewGuid();

            // Log action type
            var actionType = ActionType.ADD_INDICATOR;

            // Log description
            var description = $"Indicator added ({indicator.IndicatorId})";

            // Add user log
            var userLog = new UserLog(logId, indicator.UserId, actionType, description, now);

            // Add user log
            _mainDbContext.UserLogs.Add(userLog);

            // Save
            await _mainDbContext.SaveChangesAsync();            

            // Get indicator
            indicator = await _mainDbContext.Indicators
                .Include(x => x.Dependencies)
                .ThenInclude(x=>x.Dependency)
                .FirstOrDefaultAsync(x=>x.IndicatorId == indicator.IndicatorId);

            // Response
            var response = _mapper.Map<IndicatorResponse>(indicator);

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@UserId}, {@Request}, {@Response}", nameof(AddIndicator), logId, request.UserId, request, response);

            // Return
            return response;
        }
        public async Task<IndicatorResponse> UpdateIndicator(UpdateIndicatorRequest request)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(UpdateIndicator));
            span.AddTag("UserId", request.UserId);

            // Get indicator
            var indicator = await _mainDbContext.Indicators
                .Include(x => x.Dependencies)
                .FirstOrDefaultAsync(x => x.IndicatorId == request.IndicatorId);

            // Indicator not found
            if (indicator == null) throw new NotFoundException(IndicatorMessage.IndicatorNotFound);

            // Get dependencies
            var newDependencies = await GetIndicators(request.Dependencies);

            // Build new indicator dependencies
            var newIndicatorDependencies = IndicatorDependencyBuilder.BuildIndicatorDependencies(indicator.IndicatorId, newDependencies);

            // Get current indicator dependencies 
            var currentIndicatorDependencies = indicator.Dependencies;

            // Update dependencies
            _mainDbContext.UpdateCollection(currentIndicatorDependencies, newIndicatorDependencies);

            // Update indicator
            indicator.Update(request.Name, request.Description, request.Formula);

            // Update
            _mainDbContext.Indicators.Update(indicator);

            // Now
            var now = DateTime.UtcNow.StripSeconds();

            // Log Id
            var logId = Guid.NewGuid();

            // Log action type
            var actionType = ActionType.UPDATE_INDICATOR;

            // Log description
            var description = $"Indicator updated ({indicator.IndicatorId})";

            // Add user log
            var userLog = new UserLog(logId, indicator.UserId, actionType, description, now);

            // Add user log
            _mainDbContext.UserLogs.Add(userLog);

            // Save
            await _mainDbContext.SaveChangesAsync();

            // Get indicator
            indicator = await _mainDbContext.Indicators
                .Include(x => x.Dependencies)
                .ThenInclude(x => x.Dependency)
                .FirstOrDefaultAsync(x => x.IndicatorId == indicator.IndicatorId);

            // Response
            var response = _mapper.Map<Responses.IndicatorResponse>(indicator);

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@UserId}, {@Request}, {@Response}", nameof(UpdateIndicator), logId, request.UserId, request, response);

            // Return
            return response;
        }

        public async Task<List<Indicator>> GetIndicators(List<string> indicatorIds)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetIndicators));

            var indicators = new List<Indicator>();
            foreach (var indicatorId in indicatorIds)
            {
                // Get indicator
                var indicator = await _mainDbContext.Indicators.FirstOrDefaultAsync(x=> x.IndicatorId == indicatorId);

                // Indicator not found
                if (indicator == null) throw new NotFoundException(string.Format(IndicatorDependencyMessage.IndicatorDependencyNotFound, indicatorId));

                // Detach
                _mainDbContext.Entry(indicator).State = EntityState.Detached;

                // Add
                indicators.Add(indicator);
            }

            // Return
            return indicators;
        }
        public async Task<List<Indicator>> UpdateDependencyLevels()
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(UpdateDependencyLevels));

            // Get all indicators
            var indicators = await _mainDbContext.Indicators.ToListAsync();

            // Get all indicators dependencies
            var indicatorDependencies = await _mainDbContext.IndicatorDependencies.ToListAsync();

            // Build dependency levels
            IndicatorBuilder.BuildDependencyLevels(indicators, indicatorDependencies);

            // Update
            _mainDbContext.Indicators.UpdateRange(indicators);

            // Save
            await _mainDbContext.SaveChangesAsync();

            // Build max dependency level
            var maxDependencyLevel = IndicatorBuilder.BuildMaxDependencyLevel(indicators);

            // Stop watch
            stopwatch.Stop();

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@MaxLevel}, {@ExecutionTime}", nameof(UpdateDependencyLevels), Guid.NewGuid(), maxDependencyLevel, stopwatch.Elapsed.TotalSeconds);

            // Return
            return indicators;
        }
    }
}
