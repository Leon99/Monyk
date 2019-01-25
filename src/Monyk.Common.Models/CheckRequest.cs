using System;

namespace Monyk.Common.Models
{
    public class CheckRequest
    {
        public Guid CheckId { get; set; }
        public MonitorType Type { get; set; }
        public CheckConfiguration Configuration { get; set; }
    }
}
