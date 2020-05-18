using Autofac;
using Core.Entityframework;
using Core.Entityframework.Imp;
using Core.MongoDB;
using Core.Redis;
using Core.SqlSugar;
using Core.SqlSugar.Imp;

namespace Core
{
    public class CoreModule : Module
    {
        /// <summary>
        /// 将接口注册到容器autofac中
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(RedisRepository)).As(typeof(IRedisRepository));
            builder.RegisterType(typeof(SqlSugarRepository)).As(typeof(ISqlSugarRepository));
            builder.RegisterType(typeof(EFRepository)).As(typeof(IEFRepository));
            builder.RegisterGeneric(typeof(MongoDbRepository<>)).As(typeof(IMongoDbRepository<>));
        }
    }
}
