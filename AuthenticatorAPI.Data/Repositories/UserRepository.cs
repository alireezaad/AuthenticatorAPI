using Dapper;
using AuthenticatorAPI.Data.DataAcces;
using AuthenticatorAPI.Data.Models;
using AuthenticatorAPI.Data.Repositories.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AuthenticatorAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private APIContext _context;

        public UserRepository(APIContext context)
        {
            _context = context;
        }

        public bool IsUserExist(string userName, string password)
        {
            var query = $"SELECT * FROM DBDay_Public.dbo.Tab_Pub_User WHERE Fld_User_UserName = '{userName}' AND Fld_User_Password = '{password}'";

            using (var connection = _context.CreateConnection())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }

                var user = connection.Query<UserInfo>(query);
                if (user is null)
                    return false;
                else
                    return true;
            }
        }

        public async Task<UserInfo> GetUser(string userName, string password)
        {
            var query = $"SELECT * FROM DBDay_Public.dbo.Tab_Pub_User WHERE Fld_User_UserName = '{userName}' AND Fld_User_Password = '{password}'";

            using (var connection = _context.CreateConnection())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }

                var user = await connection.QueryAsync<UserInfo>(query);
                return user.FirstOrDefault();
            }
        }
    }
}