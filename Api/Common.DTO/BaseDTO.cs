using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    public class BaseDTO
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
