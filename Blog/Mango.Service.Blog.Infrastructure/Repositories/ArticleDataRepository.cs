/*--------------------------------------------------------------------------
//
//  Copyright 2021 Chiva Chen
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//
/*--------------------------------------------------------------------------*/

using System;
using System.Threading.Tasks;
using FreeRedis;
using Mango.EntityFramework.Abstractions;
using Mango.Service.Blog.Domain.AggregateModel.ArticleDataAggregate;
using Mango.Service.Blog.Infrastructure.Config;
using Mango.Service.Blog.Infrastructure.DbContext;

namespace Mango.Service.Blog.Infrastructure.Repositories
{
    /// <summary>
    /// 文章数据仓储接口实现（同时操作redis和数据库）
    /// </summary>
    public class ArticleDataRepository : IArticleDataRepository
    {
        private readonly BlogDbContext _context;

        private readonly RedisClient _redis;

        private int _redisExpire = new TimeSpan(7, 0, 0).Seconds;

        public IUnitOfWork UnitOfWork => _context;

        public ArticleDataRepository(BlogDbContext context, RedisClient redis)
        {
            _context = context;
            _redis = redis;
        }

        /// <summary>
        /// 加到缓存（因为文章数据信息依赖于文章表所以这里不直接进行数据库操作，只做缓存操作）
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public async Task AddAsync(ArticleData o)
        {
            if (o.Id <= 0)
            {
                throw new ArgumentException(nameof(o));
            }

            await _redis.SetAsync($"{RedisConfig.Article_Like_key}:{o.Id}", o.Like, _redisExpire);
            await _redis.SetAsync($"{RedisConfig.Article_Comment_Key}:{o.Id}", o.Comment, _redisExpire);
            await _redis.SetAsync($"{RedisConfig.Article_View_Key}:{o.Id}", o.View, _redisExpire);
        }

        /// <summary>
        /// 从缓存删除（因为文章数据信息依赖于文章表所以这里不直接进行数据库操作，只做缓存操作）
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public async Task RemoveAsync(ArticleData o)
        {
            if (o.Id <= 0)
            {
                throw new ArgumentException(nameof(o));
            }

            await _redis.DelAsync($"{RedisConfig.Article_Like_key}:{o.Id}");
            await _redis.DelAsync($"{RedisConfig.Article_Comment_Key}:{o.Id}");
            await _redis.DelAsync($"{RedisConfig.Article_View_Key}:{o.Id}");
        }

        /// <summary>
        /// 更新缓存（因为文章数据信息依赖于文章表所以这里不直接进行数据库操作，只做缓存操作）
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ArticleData o)
        {
            if (o.Id <= 0)
            {
                throw new ArgumentException(nameof(o));
            }

            if (o.LikeInc != 0) await _redis.IncrByAsync($"{RedisConfig.Article_Like_key}", o.LikeInc);
            if (o.CommentInc != 0) await _redis.IncrByAsync($"{RedisConfig.Article_Comment_Key}", o.CommentInc);
            if (o.ViewInc != 0) await _redis.IncrByAsync($"{RedisConfig.Article_View_Key}", o.ViewInc);
        }

        /// <summary>
        /// 更新到数据库（从缓存同步到数据库用）
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public Task UpdateToDbAsync(ArticleData o)
        {
            _context.ArticleData.Update(o);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 从缓存查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ArticleData> GetByIdAsync(long id)
        {
            var likeKey = $"{RedisConfig.Article_Like_key}:{id}";
            var commentKey = $"{RedisConfig.Article_Comment_Key}:{id}";
            var viewKey = $"{RedisConfig.Article_View_Key}:{id}";
            if (await _redis.ExistsAsync(likeKey) && await _redis.ExistsAsync(commentKey) &&
                await _redis.ExistsAsync(viewKey))
            {
                return new ArticleData
                (
                    id,
                    await _redis.GetAsync<int>(viewKey),
                    await _redis.GetAsync<int>(commentKey),
                    await _redis.GetAsync<int>(likeKey)
                );
            }

            var ad = await _context.ArticleData.FindAsync(id);
            if (ad != null)
            {
                await AddAsync(ad);
            }

            return ad;
        }
    }
}