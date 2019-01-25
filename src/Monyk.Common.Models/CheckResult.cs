using System;

namespace Monyk.Common.Models
{
    public class CheckResult
    {
        public Guid CheckId { get; set; }
        public CheckResultStatus Status { get; set; }
        public TimeSpan CompletionTime { get; set; }
        public string Description { get; set; }
    }
}
