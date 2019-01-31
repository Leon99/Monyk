using Monyk.Common.Models;
using Monyk.GroundControl.Models;

namespace Monyk.GroundControl.Services
{
    public static class Mapper
    {
        public static CheckRequest Map(Monitor monitor)
        {
            return new CheckRequest
            {
                MonitorId = monitor.Id,
                Type = monitor.Type,
                Configuration = new CheckConfiguration
                {
                    Target = monitor.Target,
                }
            };
        }
    }
}
