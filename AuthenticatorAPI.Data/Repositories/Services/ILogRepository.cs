using AuthenticatorAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticatorAPI.Data.Repositories.Services
{
    public interface ILogRepository
    {
        public bool InsertLog(Log log);
    }
}
