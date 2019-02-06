using Microsoft.Extensions.DependencyInjection;

namespace Monyk.Probe.Checkers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCheckers(this IServiceCollection services)
        {
            return services
                .AddSingleton<CheckerFactory>()
                .AddSingleton<IPingFactory, PingFactory>()
                .AddSingleton<IChecker, PingChecker>()
                .AddSingleton<IChecker, HttpChecker>();
        }
    }
}
