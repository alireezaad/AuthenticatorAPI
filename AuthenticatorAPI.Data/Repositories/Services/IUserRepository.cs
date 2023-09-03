using AuthenticatorAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticatorAPI.Data.Repositories.Services
{
    public interface IUserRepository
    {
        public bool IsUserExist(string userName, string password);
        public Task<UserInfo> GetUser(string userName, string password);
    }
}
