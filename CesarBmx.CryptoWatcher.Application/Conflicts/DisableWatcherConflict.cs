using CesarBmx.Shared.Application.Responses;
using Newtonsoft.Json;

namespace CesarBmx.CryptoWatcher.Application.Conflicts
{
    public enum DisableWatcherConflictReason
    {
        WATCHER_ALREADY_DISABLED,
    }

    public class DisableWatcherConflict : Error
    {
        [JsonProperty(Order = 2)]
        public DisableWatcherConflictReason Reason { get; set; }

        public DisableWatcherConflict(DisableWatcherConflictReason reason, string message)
        : base(409, message)
        {
            Reason = reason;
        }
    }
}

