using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Monyk.GroundControl.Db;
using Monyk.GroundControl.Services;

namespace Monyk.GroundControl.Main.Services
{
    public class Launcher
    {
        private readonly GroundControlDbContext _db;
        private readonly MonitorScheduler _scheduler;
        private readonly Logger<Launcher> _logger;

        public Launcher(GroundControlDbContext db, MonitorScheduler scheduler, Logger<Launcher> logger)
        {
            _db = db;
            _scheduler = scheduler;
            _logger = logger;
        }

        public void Prepare(IHostingEnvironment env)
        {
            _logger.LogInformation("Preparing Ground Control");
            Directory.SetCurrentDirectory(env.ContentRootPath); // To make it consistent across different hosts

            if (env.IsDevelopment())
            {
                Bootstrapper.SeedDataForDevelopment(_db);
            }
            else
            {
                _db.Database.Migrate();
            }
        }

        public void StartMonitoring()
        {
            var monitors = _db.Monitors.Where(m => !m.IsDeleted && !m.IsStopped);
            _logger.LogInformation($"Initial scheduling for {monitors.Count()} monitors");
            foreach (var monitor in monitors)
            {
                _scheduler.AddSchedule(monitor);
            }
        }
    }
}
