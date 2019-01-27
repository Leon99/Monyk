using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monyk.Common.Communicator;
using RabbitMQ.Client;

namespace Monyk.Common.Startup
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMQConnectionFactory(this IServiceCollection services, IConfiguration configuration)
        {
            var communicatorSettings = new CommunicatorSettings();
            configuration.Bind("Monyk.Common.Communicator", communicatorSettings);
            var factory = new ConnectionFactory
            {
                HostName = communicatorSettings.RabbitMQ.Host,
                VirtualHost = communicatorSettings.RabbitMQ.VHost,
                UserName = communicatorSettings.RabbitMQ.UserName,
                Password = communicatorSettings.RabbitMQ.Password,
            };
            services.AddSingleton<IConnectionFactory>(factory);
            return services;
        }
    }
}
