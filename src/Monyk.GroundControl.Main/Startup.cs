﻿using System.Configuration;
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
using Monyk.GroundControl.Db.Entities;
using Monyk.GroundControl.Main.Models;
using Monyk.GroundControl.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace Monyk.GroundControl.Main
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private const string SqliteFileName = "monyk.db";
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
            AddDatabase(services);

            services.AddRabbitMQConnectionFactory(_configuration);
            services.AddSingleton<ITransmitter<CheckRequest>, Transceiver<CheckRequest>>();
            
            services.AddScoped<MonitorManager>();
            services.AddSingleton<MonitorScheduler>();
            services.AddSingleton<TimerFactory>();
        }

        private void AddDatabase(IServiceCollection services)
        {
            switch (_appSettings.Database)
            {
                case DatabaseType.Pgsql:
                    services.AddEntityFrameworkNpgsql()
                        .AddDbContext<MonykDbContext>(builder => builder.UseNpgsql(_configuration.GetConnectionString("MainDb")))
                        .BuildServiceProvider();
                    break;
                case DatabaseType.Sqlite:
                    services.AddDbContext<MonykDbContext>(options => options.UseSqlite($"Data Source=" + SqliteFileName));
                    break;
                default:
                    throw new ConfigurationErrorsException(
                        $"Unable to initialize the storage due to misconfiguration. Database setting value '{_configuration["Database"]}' is not supported.");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Directory.SetCurrentDirectory(env.ContentRootPath); // To make it consistent across different hosts
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    SeedDataForDevelopment(
                        serviceScope.ServiceProvider.GetService<MonykDbContext>(),
                        app.ApplicationServices.GetService<MonitorScheduler>());
                }
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ground Control"); });
        }

        private void SeedDataForDevelopment(MonykDbContext db, MonitorScheduler scheduler)
        {
            if (_appSettings.Database == DatabaseType.Sqlite)
            {
                if (!File.Exists(SqliteFileName))
                {
                    File.CreateText(SqliteFileName).Close();
                }
            }

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Monitors.AddRange(
                new MonitorEntity
                {
                    Type = MonitorType.Http,
                    Target = "https://github.com",
                    Interval = 5,
                    Description = "Test monitor (HTTP)",
                },
                new MonitorEntity
                {
                    Type = MonitorType.Ping,
                    Target = "github.com",
                    Interval = 5,
                    Description = "Test monitor (Ping)"
                }
            );
            db.SaveChanges();
            foreach (var monitor in db.Monitors)
            {
                scheduler.AddSchedule(monitor);
            }
        }
    }
}
