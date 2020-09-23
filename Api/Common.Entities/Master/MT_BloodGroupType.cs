using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities.Master
{
    [Table("MT_BloodGroupTypes")]
    public class MT_BloodGroupType: BaseEntity
    {
        public string BloodName { get; set; }
    }
}
