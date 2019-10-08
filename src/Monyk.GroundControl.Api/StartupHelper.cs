using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Monyk.Common.Startup;

namespace Monyk.GroundControl.Api
{
    public static class StartupHelper
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddCustomizedMvc();
            services.AddOpenApiDocument(settings =>
            {
                settings.Title = "Monyk GroundControl";
            });
            return services;
        }

        public static IApplicationBuilder UseApi(this IApplicationBuilder appBuilder)
        {
            appBuilder
                .UseForwardedHeaders()
                .UseMvc();
            appBuilder
                .UseOpenApi()
                .UseSwaggerUi3();

            return appBuilder;
        }
    }
}
