﻿/*--------------------------------------------------------------------------*/
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

using Mango.Core.Cache.Abstractions;
using Mango.EntityFramework.Abstractions;
using Mango.Service.Blog.Abstractions.Models.Entities;
using Mango.Service.Blog.Abstractions.Repositories;
using Mango.Service.Blog.Abstractions.Services;
using Mango.Service.Blog.CacheConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Services
{
    /// <summary>
    /// 定时作业服务实现
    /// </summary>
    public class JobService : IJobService
    {
        private readonly ILogger<JobService> _logger;
        private readonly ICache _cache;

        private readonly IArticleRepository _articleRepository;
        private readonly IEfContextWork _work;

        public JobService(ILogger<JobService> logger,ICache cache,IArticleRepository articleRepository,IEfContextWork efContextWork)
        {
            _logger = logger;
            _cache = cache;
            _articleRepository = articleRepository;
            _work = efContextWork;
        }

        /// <summary>
        /// 同步点赞阅读数
        /// </summary>
        /// <returns></returns>
        public async Task SynLikeAndViewAsync()
        {
            var len = await RedisHelper.LLenAsync(ArticleCacheConfig.ARTICLE_LIKE_VIEW_CACHE_KEYS);
            if (len > 0)
            {
                var keyList = new List<string>();
                while(len > 0)
                {
                    var key = await RedisHelper.LPopAsync(ArticleCacheConfig.ARTICLE_LIKE_VIEW_CACHE_KEYS);
                    keyList.Add(key);
                    len--;
                }

                //遍历键值同步到数据库
                foreach(var k in keyList)
                {
                    var fk = k.Split(':')[0];
                    switch (fk)
                    {
                        case ArticleCacheConfig.LIKE_CACHE_KEY:
                            await WriteBackLikeCacheValue(k);
                            break;
                        case ArticleCacheConfig.VIEW_CACHE_KEY:
                            await WriteBackViewCacheValue(k);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 点赞数，阅读数写回mysql
        /// </summary>
        /// <returns></returns>
        public async Task WriteBackAsync()
        {
            try
            {
                var articles = await _articleRepository.Table
                    .Where(item => item.Status == 1)
                    .ToListAsync();

                foreach (var article in articles)
                {
                    var likeKey = $"{ArticleCacheConfig.LIKE_CACHE_KEY}{article.Id}";
                    var viewKey = $"{ArticleCacheConfig.VIEW_CACHE_KEY}{article.Id}";
                    await WriteBackLikeCacheValue(article, likeKey);
                    await WriteBackViewCacheValue(article, viewKey);
                    await _work.SaveChangesAsync();

                    await RedisHelper.DelAsync(likeKey);
                    await RedisHelper.DelAsync(viewKey);
                }

                _logger.LogInformation("作业成功");

            }catch(Exception ex)
            {
                _logger.LogError($"定时写回作业异常,message:{ex.Message}");
            }
        }

        /// <summary>
        /// 写回点赞
        /// </summary>
        /// <param name="article"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task WriteBackLikeCacheValue(Article article, string key)
        {
            if(await RedisHelper.ExistsAsync(key))
            {
                article.Like = await RedisHelper.GetAsync<int>(key);
            }
        }

        /// <summary>
        /// 写回阅读
        /// </summary>
        /// <param name="article"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task WriteBackViewCacheValue(Article article, string key)
        {
            if(await RedisHelper.ExistsAsync(key))
            {
                article.View = await RedisHelper.GetAsync<int>(key);
            }
        }

        /// <summary>
        /// 写回点赞数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task WriteBackLikeCacheValue(string key)
        {
            if (await RedisHelper.ExistsAsync(key))
            {
                var id = key.Split(':')[1];
                var article = await _articleRepository.Table
                    .FirstOrDefaultAsync(item => item.Id == Convert.ToInt64(id));
                if (article != null)
                {
                    article.Like = await RedisHelper.GetAsync<int>(key);
                    await _work.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// 写回阅读数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task WriteBackViewCacheValue(string key)
        {
            if (await RedisHelper.ExistsAsync(key))
            {
                var id = key.Split(':')[1];
                var article = await _articleRepository.Table
                    .FirstOrDefaultAsync(item => item.Id == Convert.ToInt64(id));
                if (article != null)
                {
                    article.View = await RedisHelper.GetAsync<int>(key);
                    await _work.SaveChangesAsync();
                }
            }
        }
    }
}
