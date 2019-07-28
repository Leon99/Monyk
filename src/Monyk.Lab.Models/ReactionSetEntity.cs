using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monyk.Lab.Models
{
    public class ReactionSetEntity
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ReactionSetReactionEntity> ReactionSetReactions { get; set; }
    }
}
