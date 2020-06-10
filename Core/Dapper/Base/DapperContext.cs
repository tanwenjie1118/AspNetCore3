using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using DbType = SqlSugar.DbType;
namespace Hal.Core.Dapper.Base
{
    public class DapperContext
    {
        public IDbConnection Instance { get; set; }

        public DapperContext(IOptions<DapperOption> options)
        {
            switch (options.Value.ConnectionDbType)
            {
                case DbType.SqlServer :
                    Instance = new SqlConnection(options.Value.ConnectionString);
                    break;
                case DbType.MySql:
                    Instance = new MySqlConnection(options.Value.ConnectionString);
                    break;
                case DbType.Oracle:
                    Instance = new OracleConnection(options.Value.ConnectionString);
                    break;
            }
        }
    }
}
