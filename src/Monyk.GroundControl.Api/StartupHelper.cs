using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Monyk.Common.Startup;
using Swashbuckle.AspNetCore.Swagger;

namespace Monyk.GroundControl.Api
{
    public static class StartupHelper
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddCustomizedMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Monyk Ground Control API", Version = "v1"});
            });
            return services;
        }

        public static IApplicationBuilder UseApi(
            this IApplicationBuilder appBuilder)
        {
            appBuilder
                .UseForwardedHeaders()
                .UseMvc();
            appBuilder
                .UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint("./v1/swagger.json", "Monyk Ground Control"); });

            return appBuilder;
        }
    }
}
