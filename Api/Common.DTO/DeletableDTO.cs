using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    public class DeletableDTO: BaseDTO
    {
        public bool IsDeleted { get; set; }
        public virtual DateTime DeleteDate { get; set; }
    }
}
