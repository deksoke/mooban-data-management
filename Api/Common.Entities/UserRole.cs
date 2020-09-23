using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public class UserRole
    {
        public int UserId { get; set; }
        public string RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
