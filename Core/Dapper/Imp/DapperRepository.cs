using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Core.Dapper.Base;
using Dapper;
using System.Data;

namespace Core.Dapper.Imp
{
    public class DapperRepository : IDapperRepository
    {
        public readonly IDbConnection connection;
        public DapperRepository(DapperContext context)
        {
            this.connection = context.Instance;
        }

        public int Delete<T>(Expression<Func<T, bool>> func) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(Expression<Func<T, bool>> func = null)
        {
            return connection.QueryFirstOrDefault<T>("select * from " + nameof(T));
        }

        public List<T> GetList<T>(Expression<Func<T, bool>> func = null)
        {
            return connection.Query<T>("select * from " + nameof(T)).ToList();
        }

        public List<T> GetPagedList<T>(int pageIndex, int pageSize, out int totalCount, out int tatalPage, Expression<Func<T, bool>> func = null)
        {
            throw new NotImplementedException();
        }

        public int Insert<T>(T entity) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public int Insert<T>(List<T> entities) where T : class, new()
        {
            throw new NotImplementedException();
        }
    }
}
