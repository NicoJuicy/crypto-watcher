using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.CryptoWatcher.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using CesarBmx.CryptoWatcher.Application.Responses;

namespace CesarBmx.CryptoWatcher.Application.Services
{
    public class LogService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly ActivitySource _activitySource;

        public LogService(
            MainDbContext mainDbContext,
            ILogger<UserService> logger,
            IMapper mapper,
            ActivitySource activitySource)
        {
            _mainDbContext = mainDbContext;
            _logger = logger;
            _mapper = mapper;
            _activitySource = activitySource;
        }

        public async Task<List<LogResponse>> GetLogs(string userId)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetLogs));

            // Get all users
            var users = await _mainDbContext.Logs.Where(x => x.UserId == userId).ToListAsync();

            // Response
            var response = _mapper.Map<List<LogResponse>>(users);

            // Return
            return response;
        }
    }
}
