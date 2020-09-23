using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities.Master
{
    [Table("STD_Moobans")]
    public class STD_Mooban: DeletableEntity
    {
        public string MoobanName { get; set; }
        public string MoobanAddress { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }
}
