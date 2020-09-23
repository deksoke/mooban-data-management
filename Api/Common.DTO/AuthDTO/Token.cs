using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO.AuthDTO
{
    public class Token
    {
        public double Expires_in;

        public string Access_token;

        public string Refresh_token;
    }
}
