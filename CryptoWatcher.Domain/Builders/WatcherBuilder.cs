﻿using System.Collections.Generic;
using System.Linq;
using CryptoWatcher.Domain.Models;


namespace CryptoWatcher.Domain.Builders
{
    public static class WatcherBuilder
    {
        public static WatcherStatus BuildStatus(decimal indicatorValue, WatcherSettings settings)
        {
            // Evaluate
            var watcherStatus = (indicatorValue >= settings.BuyAt) ? WatcherStatus.Buy : WatcherStatus.Sell;

            // Return
            return watcherStatus;
        }
        public static List<Watcher> BuildUserWatchersWithDefaults(this List<Watcher> userWatchers, string userId, List<Currency> currencies)
        {
            var watchers = new List<Watcher>();
            foreach (var currency in currencies)
            {
                // Get matching price change watcher
                var priceChangeWatcher = userWatchers.FirstOrDefault(x =>
                    x.IndicatorType == IndicatorType.PriceChange &&
                    x.CurrencyId == currency.Id);

                // If the watcher does not exist, we add the default one
                if (priceChangeWatcher == null)
                {
                    priceChangeWatcher = new Watcher(
                        userId,
                        currency.Id,
                        IndicatorType.PriceChange,
                        IndicatorBuilder.BuildValue(currency, IndicatorType.PriceChange, currencies),
                        new WatcherSettings(5, 5),
                        new WatcherSettings(0, 0),
                        false);
                }

                // Add
                watchers.Add(priceChangeWatcher);

                // Get matching hype watcher
                var hypeWatcher = userWatchers.FirstOrDefault(x =>
                    x.IndicatorType == IndicatorType.Hype &&
                    x.CurrencyId == currency.Id);

                // If the watcher does not exist, we add the default one
                if (hypeWatcher == null)
                {
                    hypeWatcher = new Watcher(
                        userId,
                        currency.Id,
                        IndicatorType.PriceChange,
                        IndicatorBuilder.BuildValue(currency, IndicatorType.PriceChange, currencies),
                        new WatcherSettings(5, 5),
                        new WatcherSettings(0, 0),
                        false);
                }

                // Add
                watchers.Add(hypeWatcher);
            }

            return watchers;
        }
        public static List<Watcher> BuildDefaultWatchers(List<Currency> currencies)
        {
            var watchers = new List<Watcher>();
            foreach (var currency in currencies)
            {
                // Add price change watcher
                var priceChangeWatcher = new Watcher(
                    "master",
                    currency.Id,
                    IndicatorType.PriceChange,
                    IndicatorBuilder.BuildValue(currency, IndicatorType.PriceChange, currencies),
                    new WatcherSettings(5, 5),
                    new WatcherSettings(0, 0),
                    false);
                watchers.Add(priceChangeWatcher);

                // Add hyper watcher
                var hypeWatcher = new Watcher(
                    "master",
                    currency.Id,
                    IndicatorType.PriceChange,
                    IndicatorBuilder.BuildValue(currency, IndicatorType.PriceChange, currencies),
                    new WatcherSettings(5, 5),
                    new WatcherSettings(0, 0),
                    false);
                watchers.Add(hypeWatcher);
            }

            return watchers;
        }
    }
}
