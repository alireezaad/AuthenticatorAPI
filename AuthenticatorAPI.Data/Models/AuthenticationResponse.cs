using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticatorAPI.Data.Models
{
    public class AuthenticationResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public int UserKind { get; set; } // 0 --> null| 1 --> automation user| 2 --> lock
        public  string CustomerName { get; set; }
        public UserInfo UserInfo { get; set; }
        public Table_lock LockInfo { get; set; }
            }
}
