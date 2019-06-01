using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monyk.Common.Communicator;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RabbitMQ.Client;

namespace Monyk.Common.Startup
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomizedMvc(this IServiceCollection services)
        {
            services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddApiExplorer()
                .AddFormatterMappings()
                .AddDataAnnotations()
                .AddJsonFormatters(settings =>
                {
                    settings.Converters.Add(new StringEnumConverter());
                    settings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddCors();
        }

        public static IServiceCollection AddRabbitMQConnectionFactory(this IServiceCollection services, IConfiguration configuration)
        {
            var communicatorSettings = configuration.GetSection("Communicator").Get<CommunicatorSettings>();
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
