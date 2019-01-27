using System.IO;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monyk.Common.Communicator;
using Monyk.Common.Models;
using Monyk.Common.Startup;
using Monyk.Probe.Checkers;

namespace Monyk.Probe.Main
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddRabbitMQConnectionFactory(Configuration);
            services.AddSingleton<ITransmitter<CheckRequest>, Transceiver<CheckRequest>>();

            services.AddHostedService<BootstrapService>();

            services.AddSingleton<IReceiver<CheckRequest>, Transceiver<CheckRequest>>();
            services.AddSingleton<ITransmitter<CheckResult>, Transceiver<CheckResult>>();

            services.AddSingleton<CheckerFactory>();
            services.AddTransient<IPing, Ping>();
            services.AddTransient<IChecker, PingChecker>();
            services.AddSingleton<IChecker, HttpChecker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        }
    }
}
