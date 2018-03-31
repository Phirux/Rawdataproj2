﻿using DataRepository;
using Microsoft.Extensions.DependencyInjection;
using StackoverflowContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {

            services.AddSingleton<ISearchHistoryRepository, SearchHistoryRepository>();

            return services;
        }
    }
}