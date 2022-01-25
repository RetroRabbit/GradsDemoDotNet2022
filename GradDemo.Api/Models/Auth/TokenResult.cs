using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradDemo.Api.Models.Auth
{
    public class TokenResult
    {
        public string Token { get; set; }
        public string Bearer { get; set; }
        public DateTime? DateIssued { get; set; }
        public DateTime? DateExpires { get; set; }

        public bool IsExpired()
        {
            return DateTime.UtcNow >= DateExpires;
        }
    }
}
