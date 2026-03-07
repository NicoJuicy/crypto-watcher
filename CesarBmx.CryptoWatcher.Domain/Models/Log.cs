using CesarBmx.CryptoWatcher.Domain.Types;
using System;

namespace CesarBmx.CryptoWatcher.Domain.Models
{
    public class Log
    {
        public Guid LogId { get; private set; }
        public string UserId { get; private set; }
        public ActionType ActionType { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Log() { }
        public Log (
            Guid logId, 
            string userId,
            ActionType actionType,
            string text, 
            DateTime createdAt)
        {
            LogId = logId;
            UserId = userId;
            ActionType = actionType;
            Description = text;
            CreatedAt = createdAt;
        }
    }
}
