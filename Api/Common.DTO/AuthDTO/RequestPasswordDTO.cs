using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.DTO.AuthDTO
{
    public class RequestPasswordDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
