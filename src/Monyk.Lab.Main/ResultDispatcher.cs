using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Monyk.Common.Models;
using Monyk.GroundControl.ApiClient;
using Monyk.Lab.Db;
using Monyk.Lab.Main.Processors;

namespace Monyk.Lab.Main
{
    public class ResultDispatcher
    {
        private readonly ILogger<ResultDispatcher> _logger;
        private readonly IGroundControlApi _groundControl;
        private readonly LabDbContext _db;
        private readonly ResultProcessorFactory _factory;

        public ResultDispatcher(ILogger<ResultDispatcher> logger, IGroundControlApi groundControl, LabDbContext db, ResultProcessorFactory factory)
        {
            _logger = logger;
            _groundControl = groundControl;
            _db = db;
            _factory = factory;
        }

        public async Task ProcessResultAsync(CheckResult result)
        {
            _logger.LogInformation("Processing result of check {CheckId}", result.CheckId);
            var monitorEntity = await _groundControl.GetMonitorAsync(result.MonitorId);

            if (monitorEntity == null)
            {
                throw new ApplicationException($"Unable to retrieve details for monitor {result.MonitorId}");
            }

            var actionGroup = await _db.ActionGroups.AsNoTracking()
                .Include(ag => ag.ActionGroupActions)
                    .ThenInclude(aga => aga.Action)
                .FirstOrDefaultAsync(ag => ag.Name == monitorEntity.ActionGroup);
            if (actionGroup == null)
            {
                _logger.LogWarning("No action group {0} for monitor {1} was found", monitorEntity.ActionGroup, monitorEntity.Id);
                return;
            }
            if (actionGroup.ActionGroupActions == null)
            {
                _logger.LogWarning("No actions for action group {0} were found", monitorEntity.ActionGroup);
                return;
            }
            foreach (var action in actionGroup.ActionGroupActions.Select(aga => aga.Action))
            {
                var processor = _factory.Create(action.Processor, action.Settings);
                await processor.RunAsync(result);
            }
        }
    }
}
