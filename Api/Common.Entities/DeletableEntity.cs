using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public abstract class DeletableEntity : BaseEntity
    {
        public bool IsDeleted { get; set; }
        public virtual DateTime DeleteDate { get; set; }
    }
}
