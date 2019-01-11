using System;
using System.Collections.Generic;
using System.Diagnostics;
using Monyk.GroundControl.Db.Entities;

namespace Monyk.GroundControl.Services
{
    public class MonitorScheduler
    {
        private readonly TimerFactory _timerFactory;

        public MonitorScheduler(TimerFactory timerFactory)
        {
            _timerFactory = timerFactory;
        }

        private readonly Dictionary<Guid, (ITimer<MonitorEntity>, MonitorEntity)> _schedules = new Dictionary<Guid, (ITimer<MonitorEntity>, MonitorEntity)>();

        public void AddSchedule(MonitorEntity monitor)
        {
            var timer = _timerFactory.Create<MonitorEntity>(TimeSpan.FromSeconds(monitor.Interval), monitor, TimerElapsedHandler);
            var scheduleData = (timer, monitor);
            _schedules.Add(monitor.Id, scheduleData);
            timer.Start();
        }

        private void TimerElapsedHandler(MonitorEntity monitor)
        {
            PostValidationRequest(monitor);
        }

        private void PostValidationRequest(MonitorEntity monitor)
        {
            Debug.WriteLine($"Requesting {monitor.Id}...");
        }

        public void DeleteSchedule(Guid id)
        {
            _schedules[id].Item1.Stop();
            _schedules.Remove(id);
        }
    }
}
