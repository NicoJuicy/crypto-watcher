﻿using System;
using CryptoWatcher.Domain.Types;


namespace CryptoWatcher.Domain.Models
{
    public class Order
    {
        public int OrderId { get; private set; }
        public OrderType OrderType { get; private set; }
        public string UserId { get; private set; }
        public string CurrencyId { get; private set; }
        public decimal Quantity { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public DateTime Time { get; private set; }
        public DateTime? NotificationProcessedAt { get; private set; }

        public Order() { }
        public Order(string userId, OrderType orderType, string currencyId, decimal quantity, DateTime time)
        {
            OrderId = 0;
            OrderType = orderType;
            UserId = userId;
            CurrencyId = currencyId;
            UserId = userId;
            Quantity = quantity;
            OrderStatus = OrderStatus.PENDING;
            Time = time;
            NotificationProcessedAt = null;
        }
    }
}
