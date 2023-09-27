using System;
using System.Collections.Generic;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.CryptoWatcher.Domain.Models;


namespace CesarBmx.CryptoWatcher.Tests.Domain.FakeModels
{
    public static class FakeWatcher
    {
        public static List<Watcher> GetWatchersNotSet()
        {
            var watchers = new List<Watcher>()
            {
                new Watcher("cesarbmx", "BTC", "master.PRICE", 3000m,null,null,null, 3000m, 3000m,3000m, false, DateTime.UtcNow.StripSeconds()),
                new Watcher("cesarbmx", "BTC", "master.PRICE", 3000m,null,null,null, 3000m, 3000m,3000m, false, DateTime.UtcNow.StripSeconds())

            };
            return watchers;
        }
        public static List<Watcher> GetWatchersBuying()
        {
            var watchers = new List<Watcher>()
            {
                new Watcher("cesarbmx", "BTC", "master.PRICE", 3000m,3100m,null,100, 3000m, 3000m,3000m, true, DateTime.UtcNow.StripSeconds()),
                new Watcher("cesarbmx", "BTC", "master.PRICE", 3000m,2000m,2900,100, 3000m, 3000m,3000m, true, DateTime.UtcNow.StripSeconds())

            };
            return watchers;
        }
        public static List<Watcher> GetWatchersHoldingOrSelling()
        {
            var watchers = new List<Watcher>()
            {
                new Watcher("cesarbmx", "BTC", "master.PRICE", 3000m,3100m,null,100, 3000m, 3000m,3000m, true, DateTime.UtcNow.StripSeconds()).SetAsBuying() ,
                new Watcher("cesarbmx", "BTC", "master.PRICE", 3000m,2000m,2900,100, 3000m, 3000m,3000m, true, DateTime.UtcNow.StripSeconds()).SetAsBuying()

            };
            return watchers;
        }
        public static List<Watcher> GetWatchersSelling()
        {
            var watchers = new List<Watcher>()
            {
                new Watcher("cesarbmx", "BTC", "master.PRICE", 3000m,2900m,3500,100, 3000m, 3000m,3000m, true, DateTime.UtcNow.StripSeconds()).SetAsBuying(),
                new Watcher("cesarbmx", "BTC", "master.PRICE", 3000m,2800m,3500,100, 3000m, 3000m,3000m, true, DateTime.UtcNow.StripSeconds()).SetAsBuying()

            };
            return watchers;
        }
        public static List<Watcher> GetWatchersHolding()
        {
            var watchers = new List<Watcher>()
            {
                new Watcher("cesarbmx", "BTC", "master.PRICE", 3000m,2900m,null,100, 3000m, 3000m,3000m, true, DateTime.UtcNow.StripSeconds()).SetAsBuying(),
                new Watcher("cesarbmx", "BTC", "master.PRICE", 3000m,2800m,null,100, 3000m, 3000m,3000m, true, DateTime.UtcNow.StripSeconds()).SetAsBuying()

            };
            return watchers;
        }
        public static List<Watcher> GetWatchersSold()
        {
            var watchers = new List<Watcher>()
            {
                new Watcher("cesarbmx", "BTC", "master.PRICE", 6000m,2900m,3500,100, 3000m, 3000m,3000m, true, DateTime.UtcNow.StripSeconds()).SetAsBuying().SetAsSelling(),
                new Watcher("cesarbmx", "BTC", "master.PRICE", 2000m,2800m,3500,100, 3000m, 3000m,3000m, true, DateTime.UtcNow.StripSeconds()).SetAsBuying().SetAsSelling()

            };
            return watchers;
        }
        public static List<Watcher> GetWatchersWillingToBuyWithDifferentWeights()
        {
            var watchers = new List<Watcher>()
            {
                new Watcher("cesarbmx", "BTC", "master.PRICE", 30000m,30000m,50000,100, null, null,30000m, true, DateTime.UtcNow.StripSeconds()),
                new Watcher("cesarbmx", "BTC", "master.PRICE", 30000m,20000m,40000,200, null, null,30000m, true, DateTime.UtcNow.StripSeconds())

            };
            return watchers;
        }
        public static List<Watcher> GetWatchersWillingToSellWithDifferentWeights()
        {
            var watchers = new List<Watcher>()
            {
                new Watcher("cesarbmx", "BTC", "master.PRICE", null,30000m,50000,100, null, null,30000m, true, DateTime.UtcNow.StripSeconds()).SetAsBuying(),
                new Watcher("cesarbmx", "BTC", "master.PRICE", null,20000m,40000,200, null, null,30000m, true, DateTime.UtcNow.StripSeconds()).SetAsBuying()

            };
            return watchers;
        }
    }
}
