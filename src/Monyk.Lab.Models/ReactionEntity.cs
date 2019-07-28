using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monyk.Lab.Models
{
    public class ReactionEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ProcessorName { get; set; }
        public string ProcessorSettings { get; set; }

        public virtual ICollection<ReactionSetReactionEntity> ReactionSetReactions { get; set; }
    }
}
