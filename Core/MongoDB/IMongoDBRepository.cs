#region <<copyright>>
/* ==============================================================================
// <copyright file="IMongoDBRepository.cs" company="HomePartenter.com">
// Copyright (c) HomePartenter.com. All rights reserved.
// </copyright>
* ==============================================================================*/
#endregion

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.MongoDB
{
    /// <summary>
    /// mongo db 通用仓储接口.
    /// </summary>
    /// <typeparam name="TEntity">collection entity.</typeparam>
    public interface IMongoDbRepository<TEntity>
        where TEntity : class

    {
        IMongoCollection<TEntity> Collection { get; }

        #region 新增.

        /// <summary>
        /// 增加实体.
        /// </summary>
        /// <param name="entity">实体.</param>
        void Add(TEntity entity);

        /// <summary>
        /// 批量新增document.
        /// </summary>
        /// <param name="entities"></param>
        void BulkAdd(IList<TEntity> entities);
        #endregion

        #region 修改.

        /// <summary>
        /// 更新实体.
        /// </summary>
        /// <param name="entity">实体.</param>
        void Update(TEntity entity);

        /// <summary>
        /// 批量更新,实际上是循环处理.
        /// </summary>
        /// <param name="entities">documents.</param>
        void BulkUpdate(IList<TEntity> entities);
        #endregion

        #region 删除.

        /// <summary>
        /// 删除document.
        /// </summary>
        /// <param name="id"></param>
        void Delete(dynamic id);

        /// <summary>
        /// 清空集合.
        /// </summary>
        void Clear();

        /// <summary>
        /// 根据条件批量删除.
        /// </summary>
        /// <param name="predicate"></param>
        void Delete(Expression<Func<TEntity, bool>> predicate);
        #endregion

        /// <summary>
        /// document是否存在.
        /// </summary>
        /// <param name="predicate">查询表达式.</param>
        /// <returns>bool.</returns>
        bool IsExists(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据id获取唯一document.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>entity.</returns>
        TEntity Get(dynamic id);

        /// <summary>
        /// 得到集合中的document总数.
        /// </summary>
        /// <param name="predicate">lambda表达式.</param>
        /// <returns>记录数.</returns>
        long Count(Expression<Func<TEntity, bool>> predicate);
    }
}
