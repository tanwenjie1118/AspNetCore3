using Infrastructure.Configuration.InnerJson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration
{
    public class DatabaseOption
    {
        public MongodbOption Mongodb { get; set; }
        public SqliteOption Sqlite { get; set; }
        public MySqlOption MySql { get; set; }
    }
}
