using System;
using System.Collections.Generic;
using System.Linq;
using Monyk.Common.Models;

namespace Monyk.Probe.Checkers
{
    public class CheckerFactory
    {
        private static readonly Dictionary<MonitorType, Type> TypeMap = new Dictionary<MonitorType, Type>
        {
            {MonitorType.Http, typeof(HttpChecker)},
            {MonitorType.Ping, typeof(PingChecker)}
        };

        private readonly IEnumerable<IChecker> _checkers;

        public CheckerFactory(IEnumerable<IChecker> checkers)
        {
            _checkers = checkers;
        }

        public IChecker Create(MonitorType type)
        {
            return _checkers.Single(c => c.GetType() == TypeMap[type]);
        }
    }
}
