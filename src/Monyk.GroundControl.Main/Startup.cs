using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monyk.Common.Communicator;
using Monyk.Common.Db;
using Monyk.Common.Models;
using Monyk.Common.Startup;
using Monyk.GroundControl.Db;
using Monyk.GroundControl.Main.Models;
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
            _appSettings = _configuration.GetSection("GroundControl").Get<GroundControlSettings>();
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
                    Title = "Monyk Ground Control API",
                    Version = "v1"
                });
            });
            services.AddDatabase<GroundControlDbContext>(_appSettings.Database, "Monyk.GroundControl.Db.Migrations");

            services.AddRabbitMQConnectionFactory(_configuration);
            services.AddSingleton<ITransmitter<CheckRequest>, Transceiver<CheckRequest>>();

            services.AddScoped<MonitorManager>();
            services.AddSingleton<MonitorScheduler>();
            services.AddSingleton<TimerFactory>();
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
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Monyk Ground Control"); });
        }
    }
}
