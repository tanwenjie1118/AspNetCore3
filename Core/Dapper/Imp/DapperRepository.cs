using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Hal.Core.Dapper.Base;
using System.Data;
using Kogel.Dapper.Extension.MySql;
using System.Threading.Tasks;

namespace Hal.Core.Dapper.Imp
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
            if (func == null)
                return connection.CommandSet<T>().Delete();

            return connection.CommandSet<T>().Delete(Get(func));
        }

        public int Delete<T>(T entity) where T : class, new()
        {
            if (entity == null)
                throw new ArgumentNullException();

            return connection.CommandSet<T>().Delete(entity);
        }

        public T Get<T>(Expression<Func<T, bool>> func = null) where T : class, new()
        {
            if (func == null)
                return connection.QuerySet<T>().Get();

            return connection.QuerySet<T>().Where(func).Get();
        }

        public List<T> GetList<T>(Expression<Func<T, bool>> func = null) where T : class, new()
        {
            if (func == null)
                return connection.QuerySet<T>().ToList();

            return connection.QuerySet<T>().Where(func).ToList();
        }

        public List<T> GetPagedList<T>(int pageIndex, int pageSize, out int totalCount, out int tatalPage, Expression<Func<T, bool>> func = null)
            where T : class, new()
        {
            var list = func == null ? connection.QuerySet<T>() : connection.QuerySet<T>().Where(func);
            var plist = list.PageList(pageIndex, pageSize);
            tatalPage = plist.TotalPage;
            totalCount = plist.Total;
            return plist.Items;
        }

        public int Insert<T>(T entity) where T : class, new()
        {
            return connection.CommandSet<T>().Insert(entity);
        }

        public int Insert<T>(List<T> entities) where T : class, new()
        {
            return connection.CommandSet<T>().Insert(entities);
        }

        public async Task<int> InsertAsync<T>(T entity) where T : class, new()
        {
            return await connection.CommandSet<T>().InsertAsync(entity);
        }

        public int Update<T>(T entity) where T : class, new()
        {
            return connection.CommandSet<T>().Update(entity);
        }

        public int Update<T>(Expression<Func<T, T>> func) where T : class, new()
        {
            return connection.CommandSet<T>().Update(func);
        }
    }
}
