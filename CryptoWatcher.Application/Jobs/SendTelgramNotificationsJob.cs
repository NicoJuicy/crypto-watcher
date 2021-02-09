﻿using System;
using System.Threading.Tasks;
using CesarBmx.Shared.Logging.Extensions;
using CryptoWatcher.Application.Services;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace CryptoWatcher.Application.Jobs
{
    public class SendTelgramNotificationsJob
    {
        private readonly NotificationService _notificationService;
        private readonly ILogger<SendWhatsappNotificationsJob> _logger;

        public SendTelgramNotificationsJob(
            NotificationService notificationService,
            ILogger<SendWhatsappNotificationsJob> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task Run()
        {
            try
            {
                await _notificationService.SendTelegramNotifications();
            }
            catch (Exception ex)
            {
                // Log into Splunk
                _logger.LogSplunkInformation("SendTelgramNotifications", new
                {
                    Failed = ex.Message
                });

                // Log error into Splunk
                _logger.LogSplunkError(ex);
            }
        }
    }
}