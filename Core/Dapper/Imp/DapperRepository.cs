using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Core.Dapper.Base;
using System.Data;
using Kogel.Dapper.Extension.MySql;
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
            if (func == null)
                return connection.CommandSet<T>().Delete();
            return connection.CommandSet<T>().Delete(Get(func));
        }

        public T Get<T>(Expression<Func<T, bool>> func = null)
        {
            if (func == null)
                return connection.QuerySet<T>().Get();
            return connection.QuerySet<T>().Where(func).Get();
        }

        public List<T> GetList<T>(Expression<Func<T, bool>> func = null)
        {
            if (func == null)
                return connection.QuerySet<T>().ToList();
            return connection.QuerySet<T>().Where(func).ToList();
        }

        public List<T> GetPagedList<T>(int pageIndex, int pageSize, out int totalCount, out int tatalPage, Expression<Func<T, bool>> func = null)
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
    }
}
