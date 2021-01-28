﻿using AutoMapper;
using CryptoWatcher.Application.Automapper;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoWatcher.Api.Configuration
{
    public static class AutomapperConfig
    {
        public static IServiceCollection ConfigureAutomapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CurrencyMapping).Assembly);

            return services;
        }
    }
}
