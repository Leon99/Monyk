using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monyk.Common.Communicator;
using Monyk.Common.Db;
using Monyk.Common.Models;
using Monyk.Common.Startup;
using Monyk.GroundControl.ApiClient;
using Monyk.Lab.Db;
using Monyk.Lab.Main.Models;
using Monyk.Lab.Main.Processors;
using Refit;
using Swashbuckle.AspNetCore.Swagger;

namespace Monyk.Lab.Main
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly LabSettings _appSettings;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = _configuration.GetSection("Lab").Get<LabSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomizedMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Monyk Lab API",
                    Version = "v1"
                });
            });
            services.AddDatabase<LabDbContext>(_appSettings.Database, "Monyk.Lab.Db.Migrations");

            services.AddHttpClient();

            services
                .AddRabbitMQConnectionFactory(_configuration)
                .AddSingleton<IReceiver<CheckResult>, Transceiver<CheckResult>>();
            services.AddScoped<ResultDispatcher>();
            services.AddSingleton<ResultProcessorFactory>();
            services
                .AddRefitClient<IGroundControlApi>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(_appSettings.GroundControlBaseUrl));

            services.AddHostedService<Launcher>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseMvc()
                .UseMiddleware<SerilogMiddleware>();
            app
                .UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Monyk Lab"); });

        }
    }
}
