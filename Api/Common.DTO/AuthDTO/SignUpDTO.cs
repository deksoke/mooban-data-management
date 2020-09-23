using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.DTO.AuthDTO
{
    public class SignUpDTO : LoginDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
