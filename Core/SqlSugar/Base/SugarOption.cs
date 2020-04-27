using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SqlSugar.Base
{
    public class SugarOption : IOptions<SugarOption>
    {
        public SugarOption Value => this;

        public string ConnectionString { set; get; }
        public DbType DbType { set; get; }
        public bool AutoClose { set; get; } = true;
    }
}
