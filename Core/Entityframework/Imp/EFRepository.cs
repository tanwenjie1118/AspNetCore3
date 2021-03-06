﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Core.Entityframework.Imp
{
    public class EFRepository : IEFRepository
    {
        private readonly MyDbContext db;
        public EFRepository(MyDbContext dbContext)
        {
            this.db = dbContext;
        }
        public int Delete<T>(Expression<Func<T, bool>> func) where T : class, new()
        {
            db.RemoveRange(db.Set<T>().Where(func));
            return db.SaveChanges();
        }

        public int Delete<T>(T entity) where T : class, new()
        {
            db.Remove(entity);
            return db.SaveChanges();
        }

        public T Get<T>(Expression<Func<T, bool>> func = null) where T : class, new()
        {
            if (func == null) return db.Set<T>().FirstOrDefault();

            return db.Set<T>().Where(func).FirstOrDefault();
        }

        public List<T> GetList<T>(Expression<Func<T, bool>> func = null) where T : class, new()
        {
            if (func == null) return db.Set<T>().ToList();

            return db.Set<T>().Where(func).ToList();
        }

        public List<T> GetPagedList<T>(int pageIndex, int pageSize, out int totalCount, out int tatalPage, Expression<Func<T, bool>> func = null) where T : class, new()
        {
            var list = func == null ? db.Set<T>() : db.Set<T>().Where(func);

            totalCount = list.Count();
            tatalPage = (totalCount % pageSize) > 0 ? (totalCount / pageSize + 1) : (totalCount / pageSize);
            var plist = list.Skip((pageSize - 1) * pageSize).Take(pageSize);
            return plist.ToList();
        }

        public int Insert<T>(T entity) where T : class, new()
        {
            db.Add(entity);
            return db.SaveChanges();
        }

        public int Insert<T>(List<T> entities) where T : class, new()
        {
            db.AddRange(entities);
            return db.SaveChanges();
        }

        public Task<int> InsertAsync<T>(T entity) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public int Update<T>(T entity) where T : class, new()
        {
            db.Update(entity);
            return db.SaveChanges();
        }

        public int Update<T>(Expression<Func<T, T>> func) where T : class, new()
        {
            var entities = db.Set<T>().Select(func);
            db.Set<T>().UpdateRange(entities);
            return db.SaveChanges();
        }
    }
}
