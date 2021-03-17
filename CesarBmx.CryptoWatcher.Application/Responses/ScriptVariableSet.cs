﻿
using System;
using System.Collections.Generic;

namespace CesarBmx.CryptoWatcher.Application.Responses
{
    public class ScriptVariableSet
    {
        public DateTime[] Times { get; set; }
        public string[] Currencies { get; set; }
        public string[] Indicators { get; set; }
        public Dictionary<DateTime, Dictionary<string, Dictionary<string, decimal>>> Values { get; set; }
    }
}
