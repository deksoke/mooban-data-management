using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace Common.Entities
{
    [Table("Users")]
    public class User: DeletableEntity
    {
        public string Username { get; set; }
        public string PasswordHashed { get; set; }
        public string Salt { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
        public int? Age { get; set; }

        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public string AddressZipCode { get; set; }
        public double? AddressLat { get; set; }
        public double? AddressLng { get; set; }

        public virtual UserPhoto Photo { get; set; }
        public virtual Settings Settings { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserClaim> Claims { get; set; }
    }
}
