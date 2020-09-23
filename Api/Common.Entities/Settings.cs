using Dapper.Contrib.Extensions;
using System;

namespace Common.Entities
{
    [Table("Setting")]
    public class Settings : BaseEntity
    {
        public string ThemeName { get; set; }
    }
}
