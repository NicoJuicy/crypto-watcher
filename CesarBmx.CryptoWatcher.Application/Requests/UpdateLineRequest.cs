using CesarBmx.CryptoWatcher.Domain.Types;
using CesarBmx.Shared.Authentication.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace CesarBmx.CryptoWatcher.Application.Requests
{
    public class UpdateLineRequest
    {
        [JsonIgnore] [Identity] public string UserId { get; set; }
        [JsonIgnore] public Period Period { get; set; }
        [JsonIgnore] public string CurrencyId { get; set; }
        [JsonIgnore] public string IndicatorId { get; set; }
        [Required] public decimal Value { get; set; }
    }
}
