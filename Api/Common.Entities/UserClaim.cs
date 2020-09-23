using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public class UserClaim : BaseEntity
    {
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}
