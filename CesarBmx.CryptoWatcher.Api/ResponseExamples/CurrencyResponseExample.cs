﻿using System.Collections.Generic;
using CesarBmx.CryptoWatcher.Application.FakeResponses;
using CesarBmx.CryptoWatcher.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace CesarBmx.CryptoWatcher.Api.ResponseExamples
{
    public class CurrencyResponseExample : IExamplesProvider<CurrencyResponse>
    {
        public CurrencyResponse GetExamples()
        {
            return FakeCurrency.GetFake_Bitcoin();
        }
    }
    public class CurrencyListResponseExample : IExamplesProvider<List<CurrencyResponse>>
    {
        public List<CurrencyResponse> GetExamples()
        {
            return FakeCurrency.GetFake_List();
        }
    }
}