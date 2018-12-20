﻿using CoinMarketCap;
using CoinMarketCap.Core;
using CryptoWatcher.BackgroundJobs;
using CryptoWatcher.Domain.Models;
using CryptoWatcher.Persistence.Contexts;
using CryptoWatcher.Persistence.Repositories;
using CryptoWatcher.Shared.Models;
using CryptoWatcher.Shared.Providers;
using CryptoWatcher.Shared.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoWatcher.UI.Configuration
{
    public static class DependecyInjectionConfig
    {
        public static IServiceCollection ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // UseMemoryStorage
            if (bool.Parse(configuration["AppSettings:UseMemoryStorage"]))
            {
                //Contexts (UOW)
                services.AddDbContext<Shared.Contexts.IContext, MainDbContext>(options => options
                    .UseInMemoryDatabase("CryptoWatcher")
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            }
            else
            {
                //Contexts (UOW)
                services.AddDbContext<Shared.Contexts.IContext, MainDbContext>(options => options
                    .UseSqlServer(configuration.GetConnectionString("CryptoWatcher"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            }

            // Repositories
            services.AddScoped<Repository<Log>, Repository<Log>>();
            services.AddScoped<Repository<Currency>, Repository<Currency>>();
            services.AddScoped<Repository<Watcher>, Repository<Watcher>>();
            services.AddScoped<Repository<User>, Repository<User>>();
            services.AddScoped<Repository<Notification>, Repository<Notification>>();
            services.AddScoped<Repository<Order>, Repository<Order>>();
            services.AddScoped<Repository<Indicator>, Repository<Indicator>>();
            services.AddScoped<Repository<Line>, LineRepository>();
            services.AddScoped<LineRepository, LineRepository>();

            // Logger repositories
            services.AddScoped<IRepository<Log>, Repository<Log>>();
            services.AddScoped<IRepository<Currency>, LoggerRepository<Currency>>();
            services.AddScoped<IRepository<Watcher>, LoggerRepository<Watcher>>();
            services.AddScoped<IRepository<User>, LoggerRepository<User>>();
            services.AddScoped<IRepository<Notification>, LoggerRepository<Notification>>();
            services.AddScoped<IRepository<Order>, LoggerRepository<Order>>();
            services.AddScoped<IRepository<Indicator>, LoggerRepository<Indicator>>();
            services.AddScoped<IRepository<Line>, LoggerRepository<Line>>();
            services.AddScoped<ILineRepository, LineLoggerRepository>();


            // Jobs
            services.AddScoped<MainJob, MainJob>();
            services.AddScoped<UpdateCurrenciesJob, UpdateCurrenciesJob>();
            services.AddScoped<UpdateLinesJob, UpdateLinesJob>();
            services.AddScoped<UpdateDefaultWatchersJob, UpdateDefaultWatchersJob>();
            services.AddScoped<UpdateWatchersJob, UpdateWatchersJob>();
            services.AddScoped<UpdateOrdersJob, UpdateOrdersJob>();
            services.AddScoped<SendWhatsappNotificationsJob, SendWhatsappNotificationsJob>();
            services.AddScoped<SendTelgramNotifications, SendTelgramNotifications>();
            services.AddScoped<RemoveLinesJob, RemoveLinesJob>();

            // Other
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<HttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICoinMarketCapClient, CoinMarketCapClient>();

            return services;
        }
    }
}