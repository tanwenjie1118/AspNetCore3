using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.SqlSugar
{
    public interface ISqlSugarRepository
    {
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetList<T>(Expression<Func<T, bool>> func = null);
        /// <summary>
        /// 查询单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        T Get<T>(Expression<Func<T, bool>> func = null);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="tatalPage"></param>
        /// <returns></returns>
        List<T> GetPagedList<T>(int pageIndex, int pageSize, out int totalCount, out int tatalPage, Expression<Func<T, bool>> func = null);
        /// <summary>
        /// 插入单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert<T>(T entity) where T : class, new();
        /// <summary>
        /// 插入多个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert<T>(List<T> entities) where T : class, new();
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        int Delete<T>(Expression<Func<T, bool>> func) where T : class, new();
    }
}
