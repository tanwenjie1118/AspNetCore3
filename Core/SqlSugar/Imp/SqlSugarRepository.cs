using Core.SqlSugar.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SqlSugar.Imp
{
    public class SqlSugarRepository : ISqlSugarRepository
    {
        private ISqlSugarClient db;
        public SqlSugarRepository(SugarContext context)
        {
            this.db = context.Instance;
        }

        public List<T> GetList<T>()
        {
            return db.Queryable<T>().ToList();
        }
    }
}
