using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public class UserPhoto : BaseEntity
    {
        public byte[] Image { get; set; }

        public virtual User User { get; set; }
    }
}
