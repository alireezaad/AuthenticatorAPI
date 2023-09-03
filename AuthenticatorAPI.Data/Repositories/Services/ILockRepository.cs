using AuthenticatorAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticatorAPI.Data.Repositories.Services
{
    public interface ILockRepository
    {
        public Task<Table_lock> GetLocksBySerial(string serial);
        public bool IsSerialExist(string serial);
        public DateTime GetLockSupportEndDate(Int64 lockId);
        public string GetLockCustomerName(int? customerId);

    }
}
