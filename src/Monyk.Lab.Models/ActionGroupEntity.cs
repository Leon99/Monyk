﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monyk.Lab.Models
{
    public class ActionGroupEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<ActionEntity> Actions { get; set; }
    }
}
