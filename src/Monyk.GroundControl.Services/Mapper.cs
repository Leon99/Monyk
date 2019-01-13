using Monyk.Common.Communicator.Models;
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
                Target = monitor.Target
            };
        }
    }
}
