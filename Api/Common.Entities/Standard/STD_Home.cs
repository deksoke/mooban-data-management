using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities.Master
{
    [Table("STD_Homes")]
    public class STD_Home: DeletableEntity
    {
        public string HomeNo { get; set; }
        public int OwnerOfHome { get; set; }
    }
}
