using Monyk.Common.Communicator.Models;
using Monyk.Common.Models;
using Monyk.GroundControl.Db.Entities;

namespace Monyk.GroundControl.Services
{
    public static class Mapper
    {
        public static CheckRequest Map(MonitorEntity monitor)
        {
            return new CheckRequest
            {
                Type = monitor.Type,
                Configuration = new CheckConfiguration
                {
                    Target = monitor.Target,
                }
            };
        }
    }
}
