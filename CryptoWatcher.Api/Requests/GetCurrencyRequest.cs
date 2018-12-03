﻿using CryptoWatcher.Api.Responses;
using MediatR;

namespace CryptoWatcher.Api.Requests
{
    public class GetCurrencyRequest : IRequest<CurrencyResponse>
    {
        public string CurrencyId { get; set; }
    }
}
