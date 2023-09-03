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
    public class LogRepository : ILogRepository
    {
        #region Variables

        private APIContext _context;

        #endregion

        public LogRepository(APIContext context)
        {
            _context = context;
        }
        public bool InsertLog(Log log)
        {
            try
            {
                string query = $@"INSERT INTO TableLog (RemoteOne,RemoteTwo,RemoteType,DataId,DataType,Date) VALUES('{log.RemoteOne}','{log.RemoteTwo}',{log.RemoteType},{log.DataId},{log.DataType},GETDATE())";
                using (var connection = _context.CreateConnection())
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    connection.Execute(query);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
