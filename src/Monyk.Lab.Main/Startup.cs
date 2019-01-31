﻿using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monyk.Common.Communicator;
using Monyk.Common.Models;
using Monyk.Common.Startup;
using Monyk.GroundControl.ApiClient;
using Refit;

namespace Monyk.Lab.Main
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddRabbitMQConnectionFactory(_configuration);
            services.AddSingleton<IReceiver<CheckResult>, Transceiver<CheckResult>>();
            services.AddSingleton(_configuration.GetSection("Monyk.Lab.Main:ResultProcessors:SlackNotifier").Get<SlackNotifierSettings>());
            services.AddSingleton<IResultProcessor, SlackNotifier>();
            services.AddRefitClient<IGroundControlApi>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(_configuration["Monyk.Lab.Main:GroundControlBaseUrl"]));

            services.AddHostedService<LabService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

        }
    }
}