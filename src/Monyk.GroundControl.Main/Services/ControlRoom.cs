using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Monyk.GroundControl.Db;
using Monyk.GroundControl.Services;

namespace Monyk.GroundControl.Main.Services
{
    public class ControlRoom
    {
        public void StartFlights(IApplicationBuilder app, IHostingEnvironment env)
        {
            Directory.SetCurrentDirectory(env.ContentRootPath); // To make it consistent across different hosts
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<MonykDbContext>();

                if (env.IsDevelopment())
                {
                    Bootstrapper.SeedDataForDevelopment(db);
                }

                ScheduleMonitors(db, app.ApplicationServices.GetService<MonitorScheduler>());
            }
        }

        private void ScheduleMonitors(MonykDbContext db, MonitorScheduler scheduler)
        {
            foreach (var monitor in db.Monitors.Where(m => !m.IsDeleted && !m.IsStopped))
            {
                scheduler.AddSchedule(monitor);
            }
        }
    }
}
