using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public class MT_Menu: DeletableEntity
    {
        [Key]
        public override int Id { get; set; }
        public string MenuName { get; set; }
        public int? ParentMenuID { get; set; }
    }
}
