#region <<copyright>>
/* ==============================================================================
// <copyright file="MongoDBRepository.cs" company="HomePartenter.com">
// Copyright (c) HomePartenter.com. All rights reserved.
// </copyright>
* ==============================================================================*/
#endregion

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.MongoDB
{
    /// <summary>
    /// mongodb repository 实现类.
    /// </summary>
    /// <typeparam name="TEntity">document.</typeparam>
    public class MongoDbRepository<TEntity> : IMongoDbRepository<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 内部id.
        /// </summary>
        private const string Primarykey = "_id";

        /// <summary>
        /// 默认的文档实体标识字段.
        /// </summary>
        private const string PrimaryPropertykey = "Id";

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbRepository{TEntity}"/> class.
        /// 构造器,通过注入上下文实例化collection.
        /// </summary>
        /// <param name="mongoDbContext">mongoDbContext.</param>
        public MongoDbRepository(MongoDbContext mongoDbContext)
        {
            this.Collection = mongoDbContext.Database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        /// <summary>
        /// Gets mongo document collection.
        /// </summary>
        public IMongoCollection<TEntity> Collection { get; }

        /// <summary>
        /// 新增document.
        /// </summary>
        /// <param name="entity">mongo entity.</param>
        public void Add(TEntity entity)
        {
            this.Collection.InsertOne(entity);
        }

        /// <summary>
        /// 批量新增document.
        /// </summary>
        /// <param name="entities">文档 集合.</param>
        public void BulkAdd(IList<TEntity> entities)
        {
            this.Collection.InsertMany(entities);
        }

        /// <summary>
        /// 更新.
        /// </summary>
        /// <param name="entity">document.</param>
        public void Update(TEntity entity)
        {
            // 获取实体Id 的value.
            var property = typeof(TEntity).GetProperties().FirstOrDefault(t => t.Name == PrimaryPropertykey);
            var idVal = property.GetValue(entity);
            if (idVal == null)
            {
                throw new Exception("entity has not id field.");
            }

            var filter = Builders<TEntity>.Filter.Eq(Primarykey, idVal);
            this.Collection.ReplaceOne(filter, entity);
        }

        /// <summary>
        /// 批量更新,实际上是循环处理.
        /// </summary>
        /// <param name="entities">documents.</param>
        public void BulkUpdate(IList<TEntity> entities)
        {
            foreach (var item in entities)
            {
                var prop = typeof(TEntity).GetProperties().FirstOrDefault(t => t.Name == PrimaryPropertykey);
                var idVal = prop.GetValue(item);
                var filter = Builders<TEntity>.Filter.Eq(Primarykey, idVal);
                this.Collection.ReplaceOne(filter, item);
            }
        }

        /// <summary>
        /// 清空集合.
        /// </summary>
        public void Clear()
        {
            var filter = Builders<TEntity>.Filter.Exists(Primarykey);
            this.Collection.DeleteMany(filter);
        }

        /// <summary>
        /// 得到集合中的document总数.
        /// </summary>
        /// <param name="predicate">lambda表达式.</param>
        /// <returns>document count.</returns>
        public long Count(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Collection.CountDocuments(predicate);
        }

        /// <summary>
        /// 根据id删除.
        /// </summary>
        /// <param name="id">id.</param>
        public void Delete(dynamic id)
        {
            var filter = Builders<TEntity>.Filter.Eq(Primarykey, id);
            this.Collection.DeleteOne(filter);
        }

        /// <summary>
        /// 根据条件批量删除.
        /// </summary>
        /// <param name="predicate">查询表达式.</param>
        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            this.Collection.DeleteMany(predicate);
        }

        /// <summary>
        /// 根据id获取唯一document.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>entity.</returns>
        public TEntity Get(dynamic id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(Primarykey, id);
            var items = this.Collection.Find(filter);
            return items.FirstOrDefault();
        }

        /// <summary>
        /// 检查实体document存在.
        /// </summary>
        /// <param name="predicate">查询表达式.</param>
        /// <returns>bool.</returns>
        public bool IsExists(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Collection.AsQueryable<TEntity>().Any(predicate);
        }
    }
}
