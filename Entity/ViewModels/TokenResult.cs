using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class TokenResult
    {
        public string Access_token { get; set; }
        public DateTime? Expiration { get; set; }
        public string UserEmail { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class TokenInfo
    {
        public string UserName { get; set; }
        public string UserGuid { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
