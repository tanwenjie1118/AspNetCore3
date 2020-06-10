using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.Core.Dapper.Base
{
    public class DapperOption
    {
        public DbType ConnectionDbType { get; set; }
        public string ConnectionString { get; set; }

        public void UseSqlServer(string connectionString)
        {
            ConnectionString = connectionString;
            ConnectionDbType = DbType.SqlServer;
        }

        public void UseMysql(string connectionString)
        {
            ConnectionString = connectionString;
            ConnectionDbType = DbType.MySql;
        }

        public void UseOracle(string connectionString)
        {
            ConnectionString = connectionString;
            ConnectionDbType = DbType.Oracle;
        }
    }
}
