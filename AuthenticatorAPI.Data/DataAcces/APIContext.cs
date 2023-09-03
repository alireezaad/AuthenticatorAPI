using AuthenticatorAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AuthenticatorAPI.Data.DataAcces
{
    public class APIContext : DbContext
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public APIContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = "Data Source = server; Initial Catalog = AuthenticatorAPIDatabase; User ID = autoweb; Password = 42515362; MultipleActiveResultSets = true";
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);


        public DbSet<Customer> Customer { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Table_lock> Table_Locks { get; set; }

    }
}
