using System;
using System.Collections.Generic;
using Monyk.Common.Communicator.Models;
using Monyk.Common.Communicator.Services;
using Monyk.GroundControl.Db.Entities;

namespace Monyk.GroundControl.Services
{
    public class MonitorScheduler
    {
        private readonly TimerFactory _timerFactory;
        private readonly ITransmitter<CheckRequest> _transmitter;

        public MonitorScheduler(TimerFactory timerFactory, ITransmitter<CheckRequest> transmitter)
        {
            _timerFactory = timerFactory;
            _transmitter = transmitter;
        }

        private readonly Dictionary<Guid, (ITimer<MonitorEntity>, MonitorEntity)> _schedules = new Dictionary<Guid, (ITimer<MonitorEntity>, MonitorEntity)>();

        public void AddSchedule(MonitorEntity monitor)
        {
            var timer = _timerFactory.Create(TimeSpan.FromSeconds(monitor.Interval), monitor, TimerElapsedHandler);
            var scheduleData = (timer, monitor);
            _schedules.Add(monitor.Id, scheduleData);
            timer.Start();
        }

        private void TimerElapsedHandler(MonitorEntity monitor)
        {
            PublishCheckRequest(monitor);
        }

        private void PublishCheckRequest(MonitorEntity monitor)
        {
            _transmitter.Transmit(Mapper.Map(monitor));
        }

        public void DeleteSchedule(Guid id)
        {
            _schedules[id].Item1.Stop();
            _schedules.Remove(id);
        }
    }
}
