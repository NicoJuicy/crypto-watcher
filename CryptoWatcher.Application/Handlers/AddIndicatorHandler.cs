﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CryptoWatcher.Application.Requests;
using CryptoWatcher.Application.Responses;
using CryptoWatcher.Domain.Expressions;
using CryptoWatcher.Domain.Messages;
using CryptoWatcher.Domain.Models;
using CryptoWatcher.Shared.Contexts;
using CryptoWatcher.Shared.Exceptions;
using CryptoWatcher.Shared.Extensions;
using CryptoWatcher.Shared.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CryptoWatcher.Application.Handlers
{
    public class AddIndicatorHandler : IRequestHandler<AddIndicatorRequest, IndicatorResponse>
    {
        private readonly IContext _context;
        private readonly IRepository<Indicator> _indicatorRepository;
        private readonly ILogger<AddIndicatorRequest> _logger;
        private readonly IMapper _mapper;

        public AddIndicatorHandler(
            IContext context,
            IRepository<Indicator> indicatorRepository,
            ILogger<AddIndicatorRequest> logger,
            IMapper mapper)
        {
            _context = context;
            _indicatorRepository = indicatorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IndicatorResponse> Handle(AddIndicatorRequest request, CancellationToken cancellationToken)
        {
            // Check if it exists
            var indicator = await _indicatorRepository.GetSingle(IndicatorExpression.Indicator(request.UserId, request.Name));

            // Throw NotFound exception if it exists
            if (indicator != null) throw new NotFoundException(IndicatorMessage.IndicatorExists);

            // Add
            indicator = new Indicator(
                request.IndicatorId,
                request.UserId, 
                request.Name, 
                request.Description,
                request.Formula);
            _indicatorRepository.Add(indicator);

             // Save
             await _context.SaveChangesAsync(cancellationToken);

            // Log into Splunk
            _logger.LogSplunkInformation(request);

            // Response
            var response = _mapper.Map<IndicatorResponse>(indicator);

            // Return
            return response;
        }
    }
}