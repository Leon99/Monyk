using System;
using System.ComponentModel.DataAnnotations;

namespace Monyk.GroundControl.Db.Entities
{
    public enum MonitorType
    {
        Http,
        Ping
    }

    public class MonitorEntity
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        [Required]
        public MonitorType Type { get; set; }
        [Required]
        public int Interval { get; set; }
        [Required]
        public string Target { get; set; }
        //public dynamic Options { get; set; }
    }
}
