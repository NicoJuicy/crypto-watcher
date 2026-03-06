using CesarBmx.Shared.Authentication.Attributes;
using Newtonsoft.Json;

namespace CesarBmx.CryptoWatcher.Application.Requests
{
    public class DisableWatcherRequest
    {
        [JsonIgnore][Identity] public string UserId { get; set; }
        [JsonIgnore] public int WatcherId { get; set; }
    }
}
