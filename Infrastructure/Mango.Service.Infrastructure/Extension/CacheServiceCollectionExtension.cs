using FreeRedis;
using Mango.Core.Cache.Config;
using Mango.Core.Serialization.Extension;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Mango.Service.Infrastructure.Extension
{
    public static class CacheServiceCollectionExtension
    {
        /// <summary>
        /// 添加FreeRedisClient
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddFreeRedis(this IServiceCollection services, Action<MangoRedisOptions> options)
        {
            var op = new MangoRedisOptions();
            options(op);
            var csb = new ConnectionStringBuilder[op.Sentinels?.Length ?? 0];
            for (var i = 0; i < csb.Length; i++)
            {
                csb[i] = op.Sentinels[i];
            }
            var client = new RedisClient(op.ConnectionString, csb);
            client.Serialize = obj => obj.ToJson();
            client.Deserialize = (obj, type) => Deser(obj,type);
            services.AddSingleton<RedisClient>(client);
            return services;
        }

        private static object Deser(string obj,Type type)
        {
            var t = typeof(JsonSerializationExtension);
            var method = t.GetMethod("ToObjectAsync", new Type[] { typeof(string) });
            method = method.MakeGenericMethod(type);
            dynamic result = method.Invoke(null, new string[] { obj });
            return result.Result;
        }
    }
}
