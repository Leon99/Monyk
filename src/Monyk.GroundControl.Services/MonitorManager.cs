using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monyk.GroundControl.Db;
using Monyk.GroundControl.Models;

namespace Monyk.GroundControl.Services
{
    public class MonitorManager
    {
        private readonly MonykDbContext _db;
        private readonly MonitorScheduler _scheduler;

        public MonitorManager(MonykDbContext db, MonitorScheduler scheduler)
        {
            _db = db;
            _scheduler = scheduler;
        }

        public async Task<IEnumerable<MonitorEntity>> GetMonitorsAsReadOnly()
        {
            return await _db.Monitors
                .AsNoTracking()
                .Where(m => !m.IsDeleted)
                .ToListAsync();
        }

        public async Task<MonitorEntity> GetMonitor(Guid id)
        {
            var monitor = await _db.Monitors.FindAsync(id);

            return monitor;
        }

        public async Task<bool> TryUpdateMonitor(Guid id, MonitorEntity monitorEntity)
        {
            _db.Entry(monitorEntity).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonitorExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        private bool MonitorExists(Guid id)
        {
            return _db.Monitors
                .AsNoTracking()
                .Any(e => e.Id == id);
        }


        public async Task CreateMonitor(MonitorEntity monitorEntity)
        {
            _db.Monitors.Add(monitorEntity);
            await _db.SaveChangesAsync();
            _scheduler.AddSchedule(monitorEntity);
        }

        public async Task<bool> TryDeleteMonitor(Guid id)
        {
            var monitor = await _db.Monitors.FindAsync(id);
            if (monitor == null)
            {
                return false;
            }
            _scheduler.DeleteSchedule(id);

            monitor.IsDeleted = true;
            await _db.SaveChangesAsync();

            return true;
        }

    }
}
