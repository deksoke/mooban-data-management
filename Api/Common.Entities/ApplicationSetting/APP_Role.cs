using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public class APP_Role
    {
        [Key]
        public string RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
