using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monyk.Common.Models;
using Monyk.GroundControl.Db;
using Monyk.GroundControl.Models;
using Monyk.GroundControl.Services;
using Bootstrapper = Monyk.Common.Db.Bootstrapper;

namespace Monyk.GroundControl.Main
{
    public class Launcher : Microsoft.Extensions.Hosting.IHostedService
    {
        private readonly ILogger<Launcher> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHostingEnvironment _env;
        private readonly MonitorScheduler _scheduler;

        public Launcher(ILogger<Launcher> logger, IServiceScopeFactory scopeFactory, IHostingEnvironment env, MonitorScheduler scheduler)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _env = env;
            _scheduler = scheduler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Opening Ground Control");
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<GroundControlDbContext>();
                Bootstrapper.PrepareDb(_env, "GroundControl.db", db, SeedDataForDevelopment);
                StartMonitoring(db);
            }

            return Task.CompletedTask;
        }

        private static void SeedDataForDevelopment(GroundControlDbContext db)
        {
            db.Monitors.AddRange(
                new MonitorEntity
                {
                    Type = MonitorType.Http,
                    Target = "https://github.com",
                    Interval = 10,
                    Description = "Happy HTTP monitor",
                    ActionGroup = "test-group-1"
                },
                new MonitorEntity
                {
                    Type = MonitorType.Http,
                    Target = "https://foo.bar",
                    Interval = 55,
                    Description = "Sad HTTP monitor",
                    ActionGroup = "test-group-1"
                },
                new MonitorEntity
                {
                    Type = MonitorType.Ping,
                    Target = "github.com",
                    Interval = 55,
                    Description = "Happy Ping monitor",
                    ActionGroup = "test-group-1"
                },
                new MonitorEntity
                {
                    Type = MonitorType.Ping,
                    Target = "foo.bar",
                    Interval = 55,
                    Description = "Sad Ping monitor",
                    ActionGroup = "test-group-2"
                },
                new MonitorEntity
                {
                    Type = MonitorType.Ping,
                    Target = "stopped.foo.bar",
                    Interval = 5,
                    IsStopped = true,
                    Description = "Stopped Ping monitor",
                    ActionGroup = "test-group-2"
                },
                new MonitorEntity
                {
                    Type = MonitorType.Ping,
                    Target = "deleted.foo.bar",
                    Interval = 5,
                    IsDeleted = true,
                    Description = "Deleted Ping monitor",
                    ActionGroup = "test-group-2"
                }
            );
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Shutting down Lab");
            return Task.CompletedTask;
        }

        public void StartMonitoring(GroundControlDbContext db)
        {
            var monitors = db.Monitors.Where(m => !m.IsDeleted && !m.IsStopped);
            _logger.LogInformation($"Initial scheduling for {monitors.Count()} monitors");
            foreach (var monitor in monitors)
            {
                _scheduler.AddSchedule(monitor);
            }
        }
    }
}
