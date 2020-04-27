using Microsoft.Extensions.Options;
using SqlSugar;

namespace Core.SqlSugar.Base
{
    public class SugarContext
    {
        public ISqlSugarClient Instance { get; set; }
        public SugarOption Option { get; set; }

        public SugarContext(IOptions<SugarOption> option)
        {
            this.Option = Option;
            var config = new ConnectionConfig()
            {
                ConnectionString = option.Value.ConnectionString,
                DbType = option.Value.DbType,
                IsAutoCloseConnection = option.Value.AutoClose,
            };

            Instance = new SqlSugarClient(config);
        }
    }
}
