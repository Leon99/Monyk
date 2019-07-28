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
            var action1 = new ReactionEntity
            {
                Name = "reaction-1",
                ProcessorName = nameof(WebHookNotifier),
                ProcessorSettings = JsonConvert.SerializeObject(new WebHookNotifierSettings {Url = "https://hooks.slack.com/services/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"})
            };
            var action2 = new ReactionEntity
            {
                Name = "reaction-2",
                ProcessorName = nameof(WebHookNotifier),
                ProcessorSettings = JsonConvert.SerializeObject(new WebHookNotifierSettings {Url = "https://hooks.slack.com/services/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"})
            };
            var action3 = new ReactionEntity {Name = "reaction-3", ProcessorName = nameof(NullResultProcessor)};
            db.Reactions.Add(action1);
            db.Reactions.Add(action2);
            db.Reactions.Add(action3);
            var reactionSet1 = new ReactionSetEntity
            {
                Name = "reactionSet-1", ReactionSetReactions = new[] {new ReactionSetReactionEntity {Reaction = action1}, new ReactionSetReactionEntity {Reaction = action2}}
            };
            var reactionSet2 = new ReactionSetEntity
            {
                Name = "reactionSet-2", ReactionSetReactions = new[] {new ReactionSetReactionEntity {Reaction = action2}, new ReactionSetReactionEntity {Reaction = action3}}
            };
            var reactionSet3 = new ReactionSetEntity {Name = "reactionSet-3"};
            db.ReactionSets.Add(reactionSet1);
            db.ReactionSets.Add(reactionSet2);
            db.ReactionSets.Add(reactionSet3);
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Shutting down Lab");
            return Task.CompletedTask;
        }
    }
}
