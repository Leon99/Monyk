using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Monyk.Common.Communicator;
using Monyk.Common.Models;
using Monyk.GroundControl.Models;

namespace Monyk.GroundControl.Services
{
    public class MonitorScheduler
    {
        private readonly ILogger<MonitorScheduler> _logger;
        private readonly TimerFactory _timerFactory;
        private readonly ITransmitter<CheckRequest> _transmitter;

        public MonitorScheduler(ILogger<MonitorScheduler> logger, TimerFactory timerFactory, ITransmitter<CheckRequest> transmitter)
        {
            _logger = logger;
            _timerFactory = timerFactory;
            _transmitter = transmitter;
        }

        private readonly Dictionary<Guid, (ITimer<Monitor>, Monitor)> _schedules = new Dictionary<Guid, (ITimer<Monitor>, Monitor)>();

        public void AddSchedule(Monitor monitor)
        {
            var timer = _timerFactory.Create(TimeSpan.FromSeconds(monitor.Interval), monitor, TimerElapsedHandler);
            var scheduleData = (timer, monitor);
            _schedules.Add(monitor.Id, scheduleData);
            timer.Start();
        }

        private void TimerElapsedHandler(Monitor monitor)
        {
            PublishCheckRequest(monitor);
        }

        private void PublishCheckRequest(Monitor monitor)
        {
            var request = Mapper.Map(monitor);
            request.CheckId = Guid.NewGuid();
            _logger.LogInformation($"Requesting check {request.CheckId}");
            _transmitter.Transmit(request);
        }

        public void DeleteSchedule(Guid id)
        {
            _schedules[id].Item1.Stop();
            _schedules.Remove(id);
        }
    }
}
