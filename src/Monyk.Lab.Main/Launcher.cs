using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monyk.Common.Communicator;
using Monyk.Common.Db;
using Monyk.Common.Models;
using Monyk.Lab.Db;
using Monyk.Lab.Main.Processors;
using Monyk.Lab.Models;

namespace Monyk.Lab.Main
{
    public class Launcher : Microsoft.Extensions.Hosting.IHostedService
    {
        private readonly ILogger<Launcher> _logger;
        private readonly IReceiver<CheckResult> _receiver;
        private readonly IEnumerable<IResultProcessor> _processors;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHostingEnvironment _env;

        public Launcher(ILogger<Launcher> logger, IReceiver<CheckResult> receiver, IEnumerable<IResultProcessor> processors, IServiceScopeFactory scopeFactory, IHostingEnvironment env)
        {
            _logger = logger;
            _receiver = receiver;
            _processors = processors;
            _scopeFactory = scopeFactory;
            _env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Opening Lab");
            _receiver.Received += async (sender, result) => await ProcessResultAsync(result);
            _receiver.StartReception();
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<LabDbContext>();
                Bootstrapper.PrepareDb(_env, "Lab.db", db, SeedDataForDevelopment);
            }
            return Task.CompletedTask;
        }

        private static void SeedDataForDevelopment(LabDbContext db)
        {
            db.ActionGroups.Add(new ActionGroupEntity { Name = "test-group-1" });
            db.ActionGroups.Add(new ActionGroupEntity { Name = "test-group-2" });
            db.ActionGroups.Add(new ActionGroupEntity { Name = "test-group-3" });
            //db.Actions.Add(new ActionEntity { });
            //db.Actions.Add(new ActionEntity { });
            //db.Actions.Add(new ActionEntity { });
        }

        private async Task ProcessResultAsync(CheckResult result)
        {
            _logger.LogInformation("Processing result of check {CheckId}", result.CheckId);
            foreach (var processor in _processors)
            {
                await processor.RunAsync(result);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Shutting down Lab");
            return Task.CompletedTask;
        }
    }
}
