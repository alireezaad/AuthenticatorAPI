using AuthenticatorAPI.Data.DataAcces;
using AuthenticatorAPI.Data.Models;
using AuthenticatorAPI.Data.Repositories.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace AuthenticatorAPI.Data.Repositories
{
    public class LockRepository : ILockRepository
    {
        APIContext _context;

        public LockRepository(APIContext context)
        {
            _context = context;
        }

        public async Task<Table_lock> GetLocksBySerial(string serial)
        {
            var query = $"SELECT * FROM Tab_Pub_lock where Fld_Lock_Serial like '{serial}%'";


            using (var connection = _context.CreateConnection())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }

                var _lock = await connection.QueryAsync<Table_lock>(query);
                return _lock.FirstOrDefault();
            }
        }

        public DateTime GetLockSupportEndDate(long lockId)
        {
            string query = $"SELECT TOP 1 CONVERT(date,Fld_KhadamatGharardad_DateEnd) FROM Tab_Pub_KhadamatGharardad WHERE Fld_Lock_ID = {lockId} ORDER BY Fld_KhadamatGharardad_ID DESC";
            using (var connection = _context.CreateConnection())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }

                var endDate = connection.Query<DateTime>(query).SingleOrDefault();
                return endDate;
            }
        }

        public bool IsSerialExist(string serial)
        {
            string query = $"SELECT * FROM Tab_Pub_lock WHERE Fld_Lock_Serial LIKE '{serial}'";
            using (var connection = _context.CreateConnection())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }

                var _lock = connection.Query<Table_lock>(query);
                if (_lock is null)
                    return false;
                else return true;
            }
        }

        public string GetLockCustomerName(int? customerId)
        {
            string query = $"SELECT ISNULL(Fld_Moshtarian_NameL1+ ' ' + Fld_Moshtarian_FamilyNameL1,Fld_Moshtarian_HoghoghiNameL1) FROM Tab_Org_Moshtarian WHERE Fld_Moshtarian_ID = {customerId}";
            using (var connection = _context.CreateConnection())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }

                var customerName = connection.Query<string>(query).SingleOrDefault();
                return customerName;
            }
        }
    }
}
