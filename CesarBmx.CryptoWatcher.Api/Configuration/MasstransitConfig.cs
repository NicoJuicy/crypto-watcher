﻿using CesarBmx.Shared.Api.Configuration;
using CesarBmx.CryptoWatcher.Persistence.Contexts;
using CesarBmx.CryptoWatcher.Application.Consumers;
using CesarBmx.Ordering.Application.Sagas;

namespace CesarBmx.CryptoWatcher.Api.Configuration
{
    public static class MasstransitConfig
    {
        public static IServiceCollection ConfigureMasstransit(this IServiceCollection services, IConfiguration configuration)
        {
            // Shared
            services.ConfigureSharedMasstransit<MainDbContext>(configuration, typeof(OrderCancelledConsumer), typeof(OrderSaga));

            // Return
            return services;
        }
        public static IApplicationBuilder ConfigureMasstransit(this IApplicationBuilder app)
        {
            // Shared
            app.ConfigureSharedMasstransit();

            return app;
        }
    }
}
