using System;
using System.Collections.Generic;
using CesarBmx.CryptoWatcher.Application.Responses;
using CesarBmx.CryptoWatcher.Domain.Types;
using CesarBmx.Shared.Common.Extensions;

namespace CesarBmx.CryptoWatcher.Application.FakeResponses
{
    public static class FakeLog
    {

        public static LogResponse GetFake_User1()
        {
            return new LogResponse
            {
                LogId = Guid.NewGuid(),
                UserId = "cesarbmx",
                ActionType = ActionType.ADD_USER,
                Description = "Watcher added (BTC, PRICE)",
                CreatedAt = DateTime.UtcNow.StripSeconds()
            };
        }
        public static List<LogResponse> GetFake_List()
        {
            return new List<LogResponse>
            {
                GetFake_User1()
            };
        }
    }
}
