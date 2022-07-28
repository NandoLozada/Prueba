using System.Data;
using System.Data.SqlClient;

namespace IntegracionWebAPI.Data
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("conexionstr");
        }

        public IDbConnection SuperConexionNando()
            => new SqlConnection(_connectionString);
    }
}
