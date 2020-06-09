using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.Domain
{
    public interface IRepository
    {
        /// <summary>
        /// Query all by where
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        abstract List<T> GetList<T>(Expression<Func<T, bool>> func = null) where T : class, new();
        /// <summary>
        /// Query one by where
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        abstract T Get<T>(Expression<Func<T, bool>> func = null)
            where T : class, new();
        /// <summary>
        /// Paged list query by where
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="tatalPage"></param>
        /// <returns></returns>
        abstract List<T> GetPagedList<T>(int pageIndex, int pageSize, out int totalCount, out int tatalPage, Expression<Func<T, bool>> func = null)
            where T : class, new();

        /// <summary>
        /// Insert an entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        abstract int Insert<T>(T entity) where T : class, new();

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        abstract int Insert<T>(List<T> entities) where T : class, new();

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        abstract int Delete<T>(Expression<Func<T, bool>> func) where T : class, new();

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        abstract int Delete<T>(T entity) where T : class, new();

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        abstract int Update<T>(T entity) where T : class, new();

        /// <summary>
        /// Update entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        abstract int Update<T>(Expression<Func<T, T>> func) where T : class, new();
    }
}
