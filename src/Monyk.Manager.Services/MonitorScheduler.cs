using System;
using System.Collections.Generic;
using System.Threading;
using Monyk.Manager.Db.Entities;

namespace Monyk.Manager.Services
{

    public class ScheduleTimer
    {

    }
    public class MonitorScheduler
    {
        private readonly Dictionary<Guid, (Timer, MonitorEntity)> _schedules = new Dictionary<Guid, (Timer, MonitorEntity)>();

        private void TimerCallback(object state)
        {
            var monitor = (MonitorEntity)state;
        }

        public void AddSchedule(MonitorEntity monitor)
        {
            var timer = new Timer(TimerCallback, monitor, TimeSpan.Zero, TimeSpan.FromSeconds(monitor.Interval));
            var scheduleData = (timer, monitor);
            _schedules.Add(monitor.Id, scheduleData);
        }

        public void DeleteSchedule(Guid id)
        {
            _schedules.Remove(id);
        }
    }
}
