using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monyk.Common.Communicator;
using Monyk.Common.Models;
using Monyk.Common.Startup;

namespace Monyk.GroundControl.Services
{
    public static class StartupHelper
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRabbitMQConnectionFactory(configuration);
            services.AddSingleton<ITransmitter<CheckRequest>, Transceiver<CheckRequest>>();

            services.AddScoped<MonitorManager>();
            services.AddSingleton<MonitorScheduler>();
            services.AddSingleton<TimerFactory>();

            return services;
        }

    }
}
