using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace Common.Entities.Master
{
    [Table("STD_Peoples")]
    public class STD_People : DeletableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdCard { get; set; }
        public DateTime DoB { get; set; }
        //public int MoobanId { get; set; }
        //public int HomeId { get; set; }
        //public string Sex { get; set; }
        //public string BloodGroup { get; set; }
        //public string Address { get; set; }
    }
}
