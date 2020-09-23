using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO.AuthDTO
{
    public class ChangePasswordDTO
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
