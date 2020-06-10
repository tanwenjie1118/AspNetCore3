using Autofac;
using Hal.Core.Dapper;
using Hal.Core.Dapper.Imp;
using Hal.Core.Entityframework;
using Hal.Core.Entityframework.Imp;
using Hal.Core.MongoDB;
using Hal.Core.Redis;
using Hal.Core.SqlSugar;
using Hal.Core.SqlSugar.Imp;

namespace Hal.Core
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
            builder.RegisterType(typeof(DapperRepository)).As(typeof(IDapperRepository));
            builder.RegisterType(typeof(EFRepository)).As(typeof(IEFRepository));
            builder.RegisterGeneric(typeof(MongoDbRepository<>)).As(typeof(IMongoDbRepository<>));
        }
    }
}
