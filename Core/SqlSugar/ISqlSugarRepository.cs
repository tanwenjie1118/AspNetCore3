using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.SqlSugar
{
    public interface ISqlSugarRepository
    {
        List<T> GetList<T>();

        T Get<T>(Expression<Func<T, bool>> func);

        int Insert<T>(T entity) where T : class, new();
    }
}
