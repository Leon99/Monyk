using System;

namespace Monyk.Lab.Models
{
    public class ActionGroupActionEntity
    {
        public Guid ActionId { get; set; }
        public Guid ActionGroupId { get; set; }
        public virtual ActionEntity Action { get; set; }
        public virtual ActionGroupEntity ActionGroup { get; set; }
    }
}
