using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Monyk.Common.Startup
{
    public static class WebHost
    {
        public static IWebHostBuilder CreateBuilder(string[] args)
        {
            var hostBuilder = new WebHostBuilder();
            if (string.IsNullOrEmpty(hostBuilder.GetSetting(WebHostDefaults.ContentRootKey)))
            {
                hostBuilder.UseContentRoot(Directory.GetCurrentDirectory());
            }

            if (args != null)
            {
                hostBuilder.UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build());
            }

            hostBuilder
                .UseKestrel((builderContext, options) => options.Configure(builderContext.Configuration.GetSection("Kestrel")))
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var hostingEnvironment = hostingContext.HostingEnvironment;
                    config
                        .AddYamlFile("appsettings.yml", true, true)
                        .AddYamlFile("appsettings." + hostingEnvironment.EnvironmentName + ".yml", true, true);
                    if (hostingEnvironment.IsDevelopment())
                    {
                        var assembly = Assembly.Load(new AssemblyName(hostingEnvironment.ApplicationName));
                        if (assembly != null)
                        {
                            config.AddUserSecrets(assembly, true);
                        }
                    }

                    config.AddEnvironmentVariables("MONYK_");
                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .WriteTo.Console()
                        .Enrich.FromLogContext()
                        ;
                    var seqUrl = hostingContext.Configuration.GetSection("Serilog:Seq")?["ServerUrl"];
                    if (seqUrl != null)
                    {
                        loggerConfiguration.WriteTo.Seq(seqUrl);
                    }
                })
                .UseDefaultServiceProvider((context, options) => options.ValidateScopes = context.HostingEnvironment.IsDevelopment());
            return hostBuilder;
        }
    }
}
