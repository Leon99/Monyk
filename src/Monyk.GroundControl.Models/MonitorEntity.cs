using System;
using System.ComponentModel.DataAnnotations;
using Monyk.Common.Models;

namespace Monyk.GroundControl.Models
{
    public class MonitorEntity
    {
        public Guid Id { get; set; }
        [Required]
        public MonitorType Type { get; set; }
        [Required]
        public string Target { get; set; }
        [Required]
        public int Interval { get; set; }
        public string Description { get; set; }
        public string ReactionSet { get; set; }
        public bool IsStopped { get; set; }
        public bool IsDeleted { get; set; }
    }
}
