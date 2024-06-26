﻿using System;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Shared.Domain.Models;


namespace CesarBmx.CryptoWatcher.Domain.Models
{
    public class Currency: IEntity<Currency>
    {
        public string Id => CurrencyId;

        public string CurrencyId { get; private set; }
        public string Name { get; private set; }
        public short Rank { get; private set; }
        public decimal Price { get; private set; }
        public decimal MarketCap { get; private set; }
        public decimal Volume24H { get; private set; }
        public decimal PercentageChange24H { get; private set; }
        public DateTime Time { get; private set; }

        public Currency() {}

        public Currency Update(Currency currency)
        {
            Rank = currency.Rank;
            Price = currency.Price;
            MarketCap = currency.MarketCap;
            Volume24H = currency.Volume24H;
            PercentageChange24H = currency.PercentageChange24H;
            Time = currency.Time;

            return this;
        }
    }
}
