using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Monyk.GroundControl.Db;
using Monyk.GroundControl.Services;

namespace Monyk.GroundControl.Main.Services
{
    public class Launcher
    {
        private readonly GroundControlDbContext _db;
        private readonly MonitorScheduler _scheduler;

        public Launcher(GroundControlDbContext db, MonitorScheduler scheduler)
        {
            _db = db;
            _scheduler = scheduler;
        }

        public void Prepare(IHostingEnvironment env)
        {
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
            foreach (var monitor in _db.Monitors.Where(m => !m.IsDeleted && !m.IsStopped))
            {
                _scheduler.AddSchedule(monitor);
            }
        }
    }
}
