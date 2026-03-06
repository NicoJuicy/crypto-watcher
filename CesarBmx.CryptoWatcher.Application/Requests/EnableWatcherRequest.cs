using CesarBmx.Shared.Authentication.Attributes;
using Newtonsoft.Json;

namespace CesarBmx.CryptoWatcher.Application.Requests
{
    public class EnableWatcherRequest
    {
        [JsonIgnore][Identity] public string UserId { get; set; }
        [JsonIgnore] public int WatcherId { get; set; }
    }
}
