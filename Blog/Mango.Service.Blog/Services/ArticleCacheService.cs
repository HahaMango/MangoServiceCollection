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
using Mango.Service.Blog.Abstractions.CacheConfig;
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
    /// 文章缓存服务实现，（写回有问题，需重新设计）
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
        /// 新增文章阅读数缓存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult> IncViewCacheAsync(IncArticleViewRequest request)
        {
            var response = new ApiResult();
            try
            {
                var key = $"{ArticleCacheConfig.VIEW_CACHE_KEY}{request.ArticleId}";
                var isExists = await RedisHelper.ExistsAsync(key);
                if (isExists)
                {
                    var view = await RedisHelper.GetAsync<int>(key);
                    if(view +1 >= ArticleCacheConfig.LIMIT_VIEW)
                    {
                        //大于阈值，持久化到数据库
                        var article = await _articleRepository.Table
                            .FirstOrDefaultAsync(item => item.Status == 1 && item.Id == request.ArticleId);
                        if(article == null)
                        {
                            await RedisHelper.DelAsync(key);
                            response.Code = Code.Error;
                            response.Message = "文章不存在或被删除";
                            return response;
                        }
                        article.View += view + 1;
                        article.UpdateTime = DateTime.Now;
                        await RedisHelper.DelAsync(key);

                        await _work.SaveChangesAsync();
                    }
                    response.Code = Code.Ok;
                    response.Message = "操作成功";
                    return response;
                }
                await RedisHelper.IncrByAsync(key);

                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"新增阅读数缓存异常;method={nameof(IncViewCacheAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"新增阅读数缓存异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 文章点赞数缓存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult> LikeCacheAsync(ArticleLikeRequest request)
        {
            var response = new ApiResult();
            try
            {
                var key = $"{ArticleCacheConfig.LIKE_CACHE_KEY}{request.ArticleId}";
                var isExists = await RedisHelper.ExistsAsync(key);
                if (isExists)
                {
                    var view = await RedisHelper.GetAsync<int>(key);
                    if (view + request.State >= ArticleCacheConfig.LIMIT_LIKE)
                    {
                        //大于阈值，持久化到数据库
                        var article = await _articleRepository.Table
                            .FirstOrDefaultAsync(item => item.Status == 1 && item.Id == request.ArticleId);
                        if (article == null)
                        {
                            await RedisHelper.DelAsync(key);
                            response.Code = Code.Error;
                            response.Message = "文章不存在或被删除";
                            return response;
                        }
                        article.View = view + request.State;
                        article.UpdateTime = DateTime.Now;
                        await RedisHelper.DelAsync(key);

                        await _work.SaveChangesAsync();
                    }
                    response.Code = Code.Ok;
                    response.Message = "操作成功";
                    return response;
                }
                await RedisHelper.IncrByAsync(key,request.State);

                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"更新点赞数缓存异常;method={nameof(LikeCacheAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"更新点赞数缓存异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 获取文章点赞缓存
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<ApiResult<int>> QueryDLikeAsync(long articleId)
        {
            var response = new ApiResult<int>();
            try
            {
                var key = $"{ArticleCacheConfig.LIKE_CACHE_KEY}{articleId}";
                var like = await RedisHelper.GetAsync<int>(key);

                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = like;
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"查询点赞数缓存异常;method={nameof(QueryDLikeAsync)};param={articleId};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询点赞数缓存异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 获取文章阅读缓存
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<ApiResult<int>> QueryDViewAsync(long articleId)
        {
            var response = new ApiResult<int>();
            try
            {
                var key = $"{ArticleCacheConfig.VIEW_CACHE_KEY}{articleId}";
                var like = await RedisHelper.GetAsync<int>(key);

                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = like;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"查询阅读数缓存异常;method={nameof(QueryDViewAsync)};param={articleId};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询阅读数缓存异常：{ex.Message}";
                return response;
            }
        }
    }
}
