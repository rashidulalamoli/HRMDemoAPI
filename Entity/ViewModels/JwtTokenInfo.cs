using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class JwtTokenInfo
    {
        public string Grant_type { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Refreshtoken { get; set; }
    }
}
