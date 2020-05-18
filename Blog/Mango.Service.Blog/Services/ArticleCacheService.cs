/*--------------------------------------------------------------------------*/
//
//  Copyright 2020 Chiva Chen
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

using Mango.Core.ApiResponse;
using Mango.Core.Cache.Abstractions;
using Mango.Core.Enums;
using Mango.Core.Serialization.Extension;
using Mango.EntityFramework.Abstractions;
using Mango.Service.Blog.CacheConfig;
using Mango.Service.Blog.Abstractions.Models.Dto;
using Mango.Service.Blog.Abstractions.Repositories;
using Mango.Service.Blog.Abstractions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Services
{
    /// <summary>
    /// 文章缓存服务实现
    /// </summary>
    public class ArticleCacheService : IArticleCacheService
    {
        private readonly ILogger<ArticleCacheService> _logger;

        private readonly IArticleRepository _articleRepository;
        private readonly IEfContextWork _work;

        /// <summary>
        /// （暂未使用）现直接使用csrediscore的Helper类访问redis
        /// </summary>
        private readonly ICache _cache;

        public ArticleCacheService(
            ILogger<ArticleCacheService> logger,
            ICache cache,
            IArticleRepository articleRepository,
            IEfContextWork efContextWork)
        {
            _logger = logger;
            _cache = cache;
            _articleRepository = articleRepository;
            _work = efContextWork;
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EditLikeAsync(ArticleLikeRequest request)
        {
            var key = $"{ArticleCacheConfig.LIKE_CACHE_KEY}{request.ArticleId}";
            if (await RedisHelper.ExistsAsync(key))
            {
                await RedisHelper.IncrByAsync(key,request.State);
            }
            await ReadLikeFromDBAsync(key);
            await RedisHelper.IncrByAsync(key,request.State);
        }

        /// <summary>
        /// 增加阅读数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task IncViewAsync(IncArticleViewRequest request)
        {
            var key = $"{ArticleCacheConfig.VIEW_CACHE_KEY}{request.ArticleId}";
            if (await RedisHelper.ExistsAsync(key))
            {
                await RedisHelper.IncrByAsync(key);
            }
            await ReadLikeFromDBAsync(key);
            await RedisHelper.IncrByAsync(key);
        }


        /// <summary>
        /// 查询点赞数缓存
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<int> QueryLikeAsync(long articleId)
        {
            var key = $"{ArticleCacheConfig.LIKE_CACHE_KEY}{articleId}";
            //缓存存在直接读缓存
            if (await RedisHelper.ExistsAsync(key))
            {
                return await RedisHelper.GetAsync<int>(key);
            }
            //缓存不存在，先读进缓存
            var like = await ReadLikeFromDBAsync(key);
            return like;
        }

        /// <summary>
        /// 查询阅读数缓存
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<int> QueryViewAsync(long articleId)
        {
            var key = $"{ArticleCacheConfig.VIEW_CACHE_KEY}{articleId}";
            //缓存存在直接读缓存
            if (await RedisHelper.ExistsAsync(key))
            {
                return await RedisHelper.GetAsync<int>(key);
            }
            //缓存不存在，先读进缓存
            var view = await ReadViewFromDBAsync(key);
            return view;
        }

        /// <summary>
        /// 从数据库读进redis
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<int> ReadLikeFromDBAsync(string key)
        {
            var id = key.Split(':')[1];
            var article = await _articleRepository.TableNotTracking
                .FirstOrDefaultAsync(item => item.Status == 1 && item.Id == Convert.ToInt64(id));
            await RedisHelper.SetAsync(key, article.Like);
            return article.Like;
        }

        /// <summary>
        /// 从数据库读进redis
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<int> ReadViewFromDBAsync(string key)
        {
            var id = key.Split(':')[1];
            var article = await _articleRepository.TableNotTracking
                .FirstOrDefaultAsync(item => item.Status == 1 && item.Id == Convert.ToInt64(id));
            await RedisHelper.SetAsync(key, article.View);
            return article.View;
        }
    }
}
