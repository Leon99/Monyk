using System;

namespace Monyk.Lab.Models
{
    public class ReactionSetReactionEntity
    {
        public Guid ReactionId { get; set; }
        public Guid ReactionSetId { get; set; }
        public virtual ReactionEntity Reaction { get; set; }
        public virtual ReactionSetEntity ReactionSet { get; set; }
    }
}
