using Core.SqlSugar.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.SqlSugar.Imp
{
    public class SqlSugarRepository : ISqlSugarRepository
    {
        private readonly ISqlSugarClient db;
        public SqlSugarRepository(SugarContext context)
        {
            this.db = context.Instance;
        }

        public T Get<T>(Expression<Func<T, bool>> func)
        {
            return db.Queryable<T>().Where(func).First();
        }

        public List<T> GetList<T>()
        {
            return db.Queryable<T>().ToList();
        }

        public int Insert<T>(T entity) where T : class, new()
        {
            return db.Insertable(entity).ExecuteReturnIdentity();
        }
    }
}
