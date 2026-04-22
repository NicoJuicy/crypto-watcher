using System;


namespace CesarBmx.CryptoWatcher.Domain.Types
{
    public enum IndicatorType
    {
        FORMULA, // Formula based indicator
        EXTERNAL, // External indicator. The value is provided externally by calling the API perdiodically 
    }
}
