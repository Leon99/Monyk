﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Monyk.Common.Models;

namespace Monyk.GroundControl.Models
{
    public class Monitor
    {
        public Guid Id { get; set; }
        [Required]
        public MonitorType Type { get; set; }
        [Required]
        public string Target { get; set; }
        [Required]
        public int Interval { get; set; }
        public string Description { get; set; }
        public bool IsSuspended { get; set; }
    }
}
