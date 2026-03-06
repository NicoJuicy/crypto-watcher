using System;


namespace CesarBmx.CryptoWatcher.Domain.Types
{
    [Flags]
    public enum ActionType
    {
        // User
        ADD_USER,

        // Watcher
        ADD_WATCHER,
        SET_WATCHER,
        ENABLE_WATCHER,
        DISABLE_WATCHER,

        // Indicator
        ADD_INDICATOR,
        UPDATE_INDICATOR,
    }
}
