using CesarBmx.CryptoWatcher.Application.Requests;
using CesarBmx.CryptoWatcher.Application.Responses;
using CesarBmx.CryptoWatcher.Application.Services;
using CesarBmx.CryptoWatcher.Domain.Types;
using CesarBmx.Shared.Api.ActionFilters;
using CesarBmx.Shared.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CesarBmx.CryptoWatcher.Api.Controllers
{
    [SwaggerResponse(500, Type = typeof(InternalServerError))]
    [SwaggerResponse(401, Type = typeof(Unauthorized))]
    [SwaggerResponse(403, Type = typeof(Forbidden))]
    [SwaggerOrder(orderPrefix: "D")]
    public class LineController : Controller
    {
        private readonly LineService _lineService;

        public LineController(LineService lineService)
        {
            _lineService = lineService;
        }

        /// <summary>
        /// Get lines
        /// </summary>
        [HttpGet]
        [Route("api/lines")]
        [SwaggerResponse(200, Type = typeof(List<Line>))]
        [SwaggerOperation(Tags = new[] { "Lines" }, OperationId = "Lines_GetLines")]
        public async Task<IActionResult> GetLines([BindRequired] Period period, List<string> currencyIds, List<string> userIds, List<string> indicatorIds)
        {
            // Reponse
            var response = await _lineService.GetLines(period, currencyIds, userIds, indicatorIds);

            // Return
            return Ok(response);
        }

        /// <summary>
        /// Update line
        /// </summary>
        [HttpPut]
        [Route("api/lines")]
        [SwaggerResponse(200, Type = typeof(IndicatorResponse))]
        [SwaggerResponse(400, Type = typeof(BadRequest))]
        [SwaggerResponse(422, Type = typeof(ValidationFailed))]
        [SwaggerOperation(Tags = new[] { "Lines" }, OperationId = "Lines_UpdateLine")]
        public async Task<IActionResult> UpdateLine([BindRequired] Period period, string currencyId, string indicatorId, [FromBody] UpdateLineRequest request)
        {
            // Request
            request.UserId = "cesarbmx";
            request.Period = period;
            request.CurrencyId = currencyId;
            request.IndicatorId = indicatorId;

            // Reponse
            var response = await _lineService.UpdateLine(request);

            // Return
            return Ok(response);
        }
    }
}

