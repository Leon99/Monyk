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

        private readonly Dictionary<Guid, (ITimer<MonitorEntity>, MonitorEntity)> _schedules = new Dictionary<Guid, (ITimer<MonitorEntity>, MonitorEntity)>();

        public void AddSchedule(MonitorEntity monitorEntity)
        {
            var timer = _timerFactory.Create(TimeSpan.FromSeconds(monitorEntity.Interval), monitorEntity, TimerElapsedHandler);
            var scheduleData = (timer, monitor: monitorEntity);
            _schedules.Add(monitorEntity.Id, scheduleData);
            timer.Start();
        }

        private void TimerElapsedHandler(MonitorEntity monitorEntity)
        {
            PublishCheckRequest(monitorEntity);
        }

        private void PublishCheckRequest(MonitorEntity monitorEntity)
        {
            var request = Mapper.Map(monitorEntity);
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
