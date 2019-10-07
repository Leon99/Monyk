using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monyk.Common.Startup;
using Monyk.GroundControl.Api;
using Monyk.GroundControl.Db;
using Monyk.GroundControl.Main.Models;
using Monyk.GroundControl.Services;

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
            services
                .AddDb(_appSettings.Database)
                .AddBusinessLogic(_configuration)
                .AddApi()
                ;

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
                .UseApi()
                .UseMiddleware<SerilogMiddleware>();
        }
    }
}
