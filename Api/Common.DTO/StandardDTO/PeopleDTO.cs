using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO.StandardDTO
{
    public class PeopleDTO: DeletableDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdCard { get; set; }
        public DateTime DoB { get; set; }
        public int MoobanId { get; set; }
        public int HomeId { get; set; }
        public string Sex { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
    }
}
