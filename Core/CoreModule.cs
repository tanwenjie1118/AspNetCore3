﻿using Autofac;
using Core.MongoDB;
using Core.Redis;
using System;
using System.Collections.Generic;
using System.Text;

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
            builder.RegisterGeneric(typeof(MongoDbRepository<>)).As(typeof(IMongoDbRepository<>));
        }
    }
}
