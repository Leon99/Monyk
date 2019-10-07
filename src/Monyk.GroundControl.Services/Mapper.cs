using Monyk.Common.Models;
using Monyk.GroundControl.Models;

namespace Monyk.GroundControl.Services
{
    internal static class Mapper
    {
        public static CheckRequest Map(MonitorEntity monitorEntity)
        {
            return new CheckRequest
            {
                MonitorId = monitorEntity.Id,
                Type = monitorEntity.Type,
                Configuration = new CheckConfiguration
                {
                    Target = monitorEntity.Target,
                }
            };
        }
    }
}
