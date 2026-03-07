using CesarBmx.Shared.Application.Responses;
using CesarBmx.CryptoWatcher.Application.Responses;
using CesarBmx.CryptoWatcher.Application.Services;
using CesarBmx.Shared.Api.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CesarBmx.CryptoWatcher.Api.Controllers
{
    [SwaggerResponse(500, Type = typeof(InternalServerError))]
    [SwaggerResponse(401, Type = typeof(Unauthorized))]
    [SwaggerResponse(403, Type = typeof(Forbidden))]
    [SwaggerOrder(orderPrefix: "F")]
    public class LogController : Controller
    {
        private readonly LogService _logService;

        public LogController(LogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Get user logs
        /// </summary>
        [HttpGet]
        [Route("api/users/{userId}/logs")]
        [SwaggerResponse(200, Type = typeof(List<LogResponse>))]
        [SwaggerOperation(Tags = new[] { "Logs" }, OperationId = "UserLogs_GetLogs")]
        public async Task<IActionResult> GetUserLogs(string userId)
        {
            // Reponse
            var response = await _logService.GetLogs(userId);

            // Return
            return Ok(response);
        }
    }
}

