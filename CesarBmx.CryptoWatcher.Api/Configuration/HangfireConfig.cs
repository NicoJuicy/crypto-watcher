﻿using CesarBmx.Shared.Api.Configuration;
using CesarBmx.CryptoWatcher.Application.Jobs;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using CesarBmx.CryptoWatcher.Application.Settings;

namespace CesarBmx.CryptoWatcher.Api.Configuration
{
    public static class HangfireConfig
    {
        public static IServiceCollection ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureSharedHangfire();

            // Grab settings
            var appSettings = configuration.GetSection<AppSettings>();
            var environmentSettings = configuration.GetSection<Shared.Settings.EnvironmentSettings>();

            if (appSettings.UseMemoryStorage)
            {
                services.AddHangfire(x => x.UseMemoryStorage());
            }
            else
            {
                services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString(appSettings.DatabaseName), new SqlServerStorageOptions
                {
                    PrepareSchemaIfNecessary = environmentSettings.Name == "Development",
                    SchemaName = "Hangfire"
                }));
            }

            // Return
            return services;
        }
        public static IApplicationBuilder ConfigureHangfire(this IApplicationBuilder app, IConfiguration configuration)
        {
            // Grab EnvironmentSettings
            var environmentSettings = configuration.GetSection<Shared.Settings.EnvironmentSettings>();

            // Enable basic auth only for Staging/Production
            app.ConfigureSharedHangfire(environmentSettings.Name == "Staging" || environmentSettings.Name == "Production");

            // Grab settings
            var appSettings = configuration.GetSection<AppSettings>();

            // Background jobs
            var jobsIntervalInMinutes = appSettings.JobsIntervalInMinutes;
            RecurringJob.AddOrUpdate<MainJob>("Main", x => x.Run(), $"*/{jobsIntervalInMinutes} * * * *");
            RecurringJob.AddOrUpdate<RemoveObsoleteLinesJob>("Remove obsolete lines", x => x.Run(), $"*/{jobsIntervalInMinutes} * * * *");

            return app;
        }
    }
}
