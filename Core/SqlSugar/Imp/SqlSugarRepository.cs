using Hal.Core.SqlSugar.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Hal.Core.SqlSugar.Imp
{
    public class SqlSugarRepository : ISqlSugarRepository
    {
        private readonly ISqlSugarClient db;
        public SqlSugarRepository(SugarContext context)
        {
            this.db = context.Instance;
        }

        public T Get<T>(Expression<Func<T, bool>> func = null)
            where T : class, new()
        {
            if (func == null) return db.Queryable<T>().First();

            return db.Queryable<T>().Where(func).First();
        }

        public List<T> GetPagedList<T>(int pageIndex, int pageSize, out int totalCount, out int tatalPage, Expression<Func<T, bool>> func = null)
            where T : class, new()
        {
            List<T> list;
            var counts = 0; var pages = 0;
            if (func == null)
            {
                list = db.Queryable<T>().ToPageList(pageIndex, pageSize, ref counts, ref pages);
            }
            else
            {
                list = db.Queryable<T>().Where(func).ToPageList(pageIndex, pageSize, ref counts, ref pages);
            }

            totalCount = counts;
            tatalPage = pages;
            return list;
        }

        public List<T> GetList<T>(Expression<Func<T, bool>> func = null) where T : class, new()
        {
            if (func == null) return db.Queryable<T>().ToList();

            return db.Queryable<T>().Where(func).ToList();
        }

        public int Insert<T>(T entity) where T : class, new()
        {
            return db.Insertable(entity).ExecuteReturnIdentity();
        }

        public int Insert<T>(List<T> entities) where T : class, new()
        {
            return db.Insertable(entities).ExecuteCommand();
        }

        public int Delete<T>(Expression<Func<T, bool>> func) where T : class, new()
        {
            return db.Deleteable<T>().Where(func).ExecuteCommand();
        }

        public int Delete<T>(T entity) where T : class, new()
        {
            return db.Deleteable(entity).ExecuteCommand();
        }

        public int Update<T>(T entity) where T : class, new()
        {
            return db.Updateable(entity).ExecuteCommand();
        }

        public int Update<T>(Expression<Func<T, T>> func) where T : class, new()
        {
            return db.Updateable(func).ExecuteCommand();
        }
    }
}
