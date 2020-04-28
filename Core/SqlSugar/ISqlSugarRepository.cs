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
        List<T> GetList<T>();
        /// <summary>
        /// 查询单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        T Get<T>(Expression<Func<T, bool>> func);
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
        List<T> Get<T>(Expression<Func<T, bool>> func, int pageIndex, int pageSize, out int totalCount, out int tatalPage);
        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert<T>(T entity) where T : class, new();
    }
}
