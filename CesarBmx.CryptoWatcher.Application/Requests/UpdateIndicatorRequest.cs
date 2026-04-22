using CesarBmx.CryptoWatcher.Domain.Types;
using CesarBmx.Shared.Authentication.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CesarBmx.CryptoWatcher.Application.Requests
{
    public class UpdateIndicatorRequest
    {
        [JsonIgnore][Identity] public string UserId { get; set; }
        public IndicatorType IndicatorType { get; set; }
        public string IndicatorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Formula { get; set; }
        public List<string> Dependencies { get; set; }
    }
}
