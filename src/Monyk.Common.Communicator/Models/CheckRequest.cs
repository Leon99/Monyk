using Monyk.Common.Models;

namespace Monyk.Common.Communicator.Models
{
    public class CheckRequest
    {
        public MonitorType Type { get; set; }
        public CheckConfiguration Configuration { get; set; }
    }
}
