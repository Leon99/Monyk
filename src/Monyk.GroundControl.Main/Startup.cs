using System.IO;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monyk.Common.Communicator;
using Monyk.Common.Models;
using Monyk.Common.Startup;
using Monyk.GroundControl.Db;
using Monyk.GroundControl.Main.Models;
using Monyk.GroundControl.Models;
using Monyk.GroundControl.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace Monyk.GroundControl.Main
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private readonly GroundControlSettings _appSettings;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = _configuration.GetSection("Monyk.GroundControl").Get<GroundControlSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddApiExplorer()
                .AddAuthorization()
                .AddFormatterMappings()
                .AddCacheTagHelper()
                .AddDataAnnotations()
                .AddJsonFormatters(settings =>
                {
                    settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                })
                .AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Monyk Ground Control API",
                    Version = "v1"
                });
            });
            services.AddDatabase(_appSettings.DatabaseType, _configuration.GetConnectionString("MainDb"));

            services.AddRabbitMQConnectionFactory(_configuration);
            services.AddSingleton<ITransmitter<CheckRequest>, Transceiver<CheckRequest>>();

            services.AddScoped<MonitorManager>();
            services.AddSingleton<MonitorScheduler>();
            services.AddSingleton<TimerFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Directory.SetCurrentDirectory(env.ContentRootPath); // To make it consistent across different hosts
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<MonykDbContext>();
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    Bootstrapper.SeedDataForDevelopment(db);
                }
                else
                {
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }
                ScheduleMonitors(db, app.ApplicationServices.GetService<MonitorScheduler>());
            }

            //app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ground Control"); });
        }

        private static void ScheduleMonitors(MonykDbContext db, MonitorScheduler scheduler)
        {
            foreach (var monitor in db.Monitors)
            {
                scheduler.AddSchedule(monitor);
            }
        }
    }
}
