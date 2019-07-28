﻿using System.Collections.Generic;
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
using Newtonsoft.Json;

namespace Monyk.Lab.Main
{
    public class Launcher : Microsoft.Extensions.Hosting.IHostedService
    {
        private readonly ILogger<Launcher> _logger;
        private readonly IReceiver<CheckResult> _receiver;
        private readonly IEnumerable<IResultProcessor> _processors;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHostingEnvironment _env;
        private readonly ResultDispatcher _dispatcher;

        public Launcher(ILogger<Launcher> logger,
            IReceiver<CheckResult> receiver,
            IEnumerable<IResultProcessor> processors,
            IServiceScopeFactory scopeFactory,
            IHostingEnvironment env)
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
            _receiver.Received += async (sender, result) =>
            {
                using (var receivedScope = _scopeFactory.CreateScope())
                {
                    var router = receivedScope.ServiceProvider.GetService<ResultDispatcher>();
                    await router.ProcessResultAsync(result);
                }
            };
            _receiver.StartReception();
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<LabDbContext>();
                Bootstrapper.PrepareDb(_env, db, SeedDataForDevelopment);
            }

            return Task.CompletedTask;
        }

        private static void SeedDataForDevelopment(LabDbContext db)
        {
            var action1 = new ActionEntity
            {
                Name = "action-1",
                Processor = nameof(WebHookNotifier),
                Settings = JsonConvert.SerializeObject(new WebHookNotifierSettings {Url = "https://hooks.slack.com/services/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"})
            };
            var action2 = new ActionEntity
            {
                Name = "action-2",
                Processor = nameof(WebHookNotifier),
                Settings = JsonConvert.SerializeObject(new WebHookNotifierSettings {Url = "https://hooks.slack.com/services/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"})
            };
            var action3 = new ActionEntity {Name = "action-3", Processor = nameof(NullResultProcessor)};
            db.Actions.Add(action1);
            db.Actions.Add(action2);
            db.Actions.Add(action3);
            var actionGroup1 = new ActionGroupEntity
            {
                Name = "actionGroup-1", ActionGroupActions = new[] {new ActionGroupActionEntity {Action = action1}, new ActionGroupActionEntity {Action = action2}}
            };
            var actionGroup2 = new ActionGroupEntity
            {
                Name = "actionGroup-2", ActionGroupActions = new[] {new ActionGroupActionEntity {Action = action2}, new ActionGroupActionEntity {Action = action3}}
            };
            var actionGroup3 = new ActionGroupEntity {Name = "actionGroup-3"};
            db.ActionGroups.Add(actionGroup1);
            db.ActionGroups.Add(actionGroup2);
            db.ActionGroups.Add(actionGroup3);
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Shutting down Lab");
            return Task.CompletedTask;
        }
    }
}
