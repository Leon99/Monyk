using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monyk.Lab.Models
{
    public class ActionEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Processor { get; set; }
        public string Settings { get; set; }

        public virtual ICollection<ActionGroupActionEntity> ActionGroupActions { get; set; }
    }
}
