using System;
using System.Collections.Generic;
using System.Linq;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.CryptoWatcher.Domain.Expressions;
using CesarBmx.CryptoWatcher.Domain.Models;
using CesarBmx.CryptoWatcher.Domain.Types;

namespace CesarBmx.CryptoWatcher.Domain.Builders
{
    public static class WatcherBuilder
    {
        public static List<Watcher> BuildDefaultWatchers(List<Line> lines)
        {
            var now = DateTime.UtcNow.StripSeconds();
            var watchers = new List<Watcher>();
            foreach (var line in lines)
            {
                // Add default watcher
                var watcher = new Watcher(
                    "master",
                    line.CurrencyId,
                    line.IndicatorId,
                    false,
                    now);

                // Set
                watcher.Set(line.AverageBuy, line.AverageSell, null);

                // Sync
                watcher.Sync(line.AverageBuy, line.AverageSell, line.Value, line.Price);

                // Add
                watchers.Add(watcher);
            }

            // Return
            return watchers;
        }
        public static void SyncWatchers(this List<Watcher> watchers, List<Watcher> defaultWatchers)
        {
            // Sync watcher
            foreach (var watcher in watchers)
            {
                var defaultWatcher = defaultWatchers.FirstOrDefault(WatcherExpression.DefaultWatcherFunc(watcher.CurrencyId, watcher.IndicatorId));
                if (defaultWatcher != null) watcher.Sync(defaultWatcher);
            }
        }
        public static decimal? BuildProfit(decimal? entryPrice, decimal? exitPrice, decimal? quantity)
        {
            // Make the calculation only when watcher has exited
            if (!entryPrice.HasValue || !exitPrice.HasValue || !quantity.HasValue) return null;

            // Result
            var result = (exitPrice - entryPrice) * quantity;

            // Return
            return result;
        }
        public static Guid BuildOrderId(this Watcher watcher)
        {
            if (watcher.SellingOrder != null) return watcher.SellingOrder.OrderId;
            if (watcher.BuyingOrder != null) return watcher.BuyingOrder.OrderId;
            throw new NotImplementedException();

        }
        public static WatcherStatus BuildWatcherStatus(WatcherStatus currentStatus, decimal? buy, decimal? sell, decimal? value, bool hasBuyingOrder, bool hasSellingOrder, bool isBuyingOrderConfirmed, bool isSellingOrderConfirmed)
        {

            // If the sell is confirmed, then it is sold 
            if (currentStatus == WatcherStatus.SELLING && hasSellingOrder && isSellingOrderConfirmed)
                return WatcherStatus.SOLD;

            // If it has a selling order, or the sell value is less than or equal to the current value, then it is selling
            if ((currentStatus == WatcherStatus.BUYING  || currentStatus == WatcherStatus.HOLDING ) && (hasSellingOrder || value >= sell))
                return WatcherStatus.SELLING;

            // If the buy is confirmed, then it is bought
            if (hasBuyingOrder && isBuyingOrderConfirmed)
                return WatcherStatus.HOLDING;

            // If the buy value is greater than or equal to the current value, or it has a buying order,  then it is buying
            if (value <= buy || hasBuyingOrder && !isBuyingOrderConfirmed)
                return WatcherStatus.BUYING;

            // If the watcher has, at least, a buy value, then it is set
            if (buy != null)
                return WatcherStatus.SET;

            // Default value
            return WatcherStatus.NOT_SET;
        }

        public static bool BuildHasBuyingOrder(this Watcher watcher)
        {
            var hasBuyingOrder = watcher.BuyingOrder != null;
            return hasBuyingOrder;
        }
        public static bool BuildHasSellingOrder(this Watcher watcher)
        {
            var hasSellingOrder = watcher.SellingOrder != null;
            return hasSellingOrder;
        }
        public static bool BuildIsBuyingOrderConfirmed(this Watcher watcher)
        {
            var isBuyingOrderConfirmed = watcher.BuyingOrder != null && watcher.BuyingOrder.ExecutedAt.HasValue;
            return isBuyingOrderConfirmed;
        }
        public static bool BuildIsSellingOrderConfirmed(this Watcher watcher)
        {
            var isSellingOrderConfirmed = watcher.SellingOrder != null && watcher.SellingOrder.ExecutedAt.HasValue;
            return isSellingOrderConfirmed;
        }
    }
}
