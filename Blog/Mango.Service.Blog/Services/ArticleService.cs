﻿/*--------------------------------------------------------------------------
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Core.ApiResponse;
using Mango.Core.DataStructure;
using Mango.Core.Serialization.Extension;
using Mango.Service.Blog.Abstractions.Models.Dto;
using Mango.Service.Blog.Abstractions.Repositories;
using Mango.Service.Blog.Abstractions.Services;
using Microsoft.Extensions.Logging;
using Mango.Core.Enums;
using Mango.EntityFramework.Abstractions;
using Mango.Service.Blog.Abstractions.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Mango.Core.Extension;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Razor.Language;
using System.Security.Cryptography.X509Certificates;

namespace Mango.Service.Blog.Services
{
    /// <summary>
    /// 文章服务实现类
    /// </summary>
    public class ArticleService : IArticleService
    {
        private readonly ILogger<ArticleService> _logger;

        private readonly IArticleRepository _articleRepository;
        private readonly IArticleDetailRepository _articleDetailRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEfContextWork _work;

        private readonly IArticleCacheService _articleCacheService;

        public ArticleService(
            ILogger<ArticleService> logger,
            IArticleRepository articleRepository,
            IArticleDetailRepository articleDetailRepository,
            IEfContextWork work,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            IArticleCacheService articleCacheService)
        {
            _logger = logger;
            _articleRepository = articleRepository;
            _articleDetailRepository = articleDetailRepository;
            _work = work;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _articleCacheService = articleCacheService;
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddArticleAsync(AddArticleRequest request, long userId)
        {
            var response = new ApiResult();
            try
            {
                var defaultCategoryId = default(long);

                var user = await _userRepository.TableNotTracking
                    .FirstOrDefaultAsync(item => item.Id == userId && item.Status == 1);
                if(user == null)
                {
                    response.Code = Code.Error;
                    response.Message = "查无用户";
                    return response;
                }
                //有传分类Id直接赋值
                if (request.CategoryId.HasValue)
                {
                    defaultCategoryId = request.CategoryId.Value;
                }
                else
                {
                    //没有传分类Id，获取默认分类。若不存在默认分类请新建
                    var category = await _categoryRepository.TableNotTracking
                        .FirstOrDefaultAsync(item => item.UserId == userId && item.Status == 1 && item.IsDefault == 1);
                    if (category == null)
                    {
                        var newCategory = new Category(true)
                        {
                            IsDefault = 1,
                            Status = 1,
                            CategoryName = "默认分类",
                            CreateTime = DateTime.Now,
                            UserId = userId,
                            Creator = user.UserName
                        };
                        await _categoryRepository.InsertAsync(newCategory);
                        category = newCategory;
                    }
                    defaultCategoryId = category.Id;
                }

                var article = new Article(true)
                {
                    Title = request.Title,
                    Describe = request.Describe,
                    CategoryId = defaultCategoryId,
                    Status = 1,
                    CreateTime = DateTime.Now,
                    Creator = user.UserName,
                    UserId = userId
                };

                var detail = new ArticleDetail(true)
                {
                    ArticleId = article.Id,
                    Content = request.Content,
                    ContentType = request.ContentType,
                    CreateTime = DateTime.Now,
                    Creator = user.UserName,
                    Status = 1
                };

                await _articleRepository.InsertAsync(article);
                await _articleDetailRepository.InsertAsync(detail);

                await _work.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "添加成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"添加文章异常;method={nameof(AddArticleAsync)};param={request?.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"添加文章异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 点赞文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult> ArticleLikeAsync(ArticleLikeRequest request)
        {
            var response = new ApiResult();
            try
            {
                await _articleCacheService.EditLikeAsync(request);

                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"点赞文章异常;method={nameof(ArticleLikeAsync)};param={request?.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"点赞文章异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 查询文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<ArticleDetailResponse>> QueryArticleDetailAsync(ArticleDetailRequest request)
        {
            var response = new ApiResult<ArticleDetailResponse>();
            try
            {
                var article = await _articleRepository.TableNotTracking
                    .FirstOrDefaultAsync(item => item.Id == request.ArticleId && item.Status == 1);
                if (article == null)
                {
                    response.Code = Code.Error;
                    response.Message = "文章不存在或被删除";
                    return response;
                }
                var detail = await _articleDetailRepository.TableNotTracking
                    .FirstOrDefaultAsync(item => item.ArticleId == request.ArticleId && item.Status == 1);
                if(detail == null)
                {
                    response.Code = Code.Error;
                    response.Message = "查无文章详情";
                    return response;
                }
                var userName = await _userRepository.TableNotTracking
                    .Where(item => item.Id == article.UserId && item.Status == 1)
                    .Select(item => item.UserName)
                    .FirstOrDefaultAsync();

                var articleDetailResponse = article.MapTo<ArticleDetailResponse>();
                articleDetailResponse.UserName = userName;
                articleDetailResponse.Content = detail.Content;
                articleDetailResponse.ContentType = detail.ContentType;
                articleDetailResponse.Like = await _articleCacheService.QueryLikeAsync(article.Id);
                articleDetailResponse.View = await _articleCacheService.QueryViewAsync(article.Id);

                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = articleDetailResponse;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"查询文章详情异常;method={nameof(QueryArticleDetailAsync)};param={request?.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询文章详情异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 查询文章分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageList<ArticlePageListResponse>>> QueryArticlePageAsync(ArticlePageRequest request)
        {
            var response = new ApiResult<PageList<ArticlePageListResponse>>();
            try
            {
                #region 拼接查询条件

                Expression<Func<Article, bool>> articleWhere = item => item.UserId == request.UserId && item.Status == 1;
                if (!string.IsNullOrEmpty(request.Title))
                {
                    articleWhere = articleWhere.And(item => item.Title == request.Title);
                }
                if (request.StartTime.HasValue && request.EndTime.HasValue)
                {
                    articleWhere = articleWhere.And(item => item.CreateTime >= request.StartTime.Value && item.CreateTime <= request.EndTime.Value);
                }

                #endregion

                var articles = await _articleRepository.TableNotTracking
                    .Where(articleWhere)
                    .OrderByDescending(item => item.IsTop)
                    .ThenByDescending(item => item.CreateTime)
                    .Select(item => new ArticlePageListResponse
                    {
                        Id = item.Id.ToString(),
                        Title = item.Title,
                        Describe = item.Describe,
                        CategoryId = item.CategoryId,
                        View=  item.View,
                        Comment = item.Comment,
                        Like = item.Like,
                        IsTop = item.IsTop,
                        CreateTime = item.CreateTime
                    })
                    .ToPageListAsync(request.PageParm.Page, request.PageParm.Size);

                foreach(var article in articles.Data)
                {
                    article.Like = await _articleCacheService.QueryLikeAsync(Convert.ToInt64(article.Id));
                    article.View = await _articleCacheService.QueryViewAsync(Convert.ToInt64(article.Id));
                }

                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = articles;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"查询文章分页异常;method={nameof(QueryArticlePageAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询文章分页异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 增加文章阅读数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult> IncViewAsync(IncArticleViewRequest request)
        {
            var response = new ApiResult();
            try
            {
                await _articleCacheService.IncViewAsync(request);

                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"增加文章阅读数异常;method={nameof(IncViewAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"增加文章阅读数异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 后台-查询文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<AdminArticleDetailResponse>> QueryAdminArticleDetailAsync(AdminArticleDetailRequest request)
        {
            var response = new ApiResult<AdminArticleDetailResponse>();
            try
            {
                var article = await _articleRepository.TableNotTracking
                    .FirstOrDefaultAsync(item => item.Id == request.ArticleId && item.Status == 1);
                if (article == null)
                {
                    response.Code = Code.Error;
                    response.Message = "文章不存在或被删除";
                    return response;
                }
                var detail = await _articleDetailRepository.TableNotTracking
                    .FirstOrDefaultAsync(item => item.ArticleId == request.ArticleId && item.Status == 1);

                var userName = await _userRepository.TableNotTracking
                    .Where(item => item.Id == article.UserId && item.Status == 1)
                    .Select(item => item.UserName)
                    .FirstOrDefaultAsync();

                var articleDetailResponse = article.MapTo<AdminArticleDetailResponse>();
                articleDetailResponse.UserName = userName;
                articleDetailResponse.Content = detail.Content;
                articleDetailResponse.ContentType = detail.ContentType;

                response.Code = Code.Ok;
                response.Message = "操作成功";
                response.Data = articleDetailResponse;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"查询文章详情异常;method={nameof(QueryAdminArticleDetailAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询文章详情异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 后台-编辑文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult> AdminEditArticleAsync(AdminEditArticleRequest request,long userId,string userName)
        {
            var response = new ApiResult();
            try
            {
                var article = await _articleRepository.Table
                    .FirstOrDefaultAsync(item => item.Id == request.ArticleId && item.Status == 1 && item.UserId == userId);
                var detail = await _articleDetailRepository.Table
                    .FirstOrDefaultAsync(item => item.ArticleId == request.ArticleId && item.Status == 1);
                if(article == null || detail == null)
                {
                    response.Code = Code.Error;
                    response.Message = "查无文章";
                    return response;
                }

                article.Title = request.Title;
                article.Describe = request.Desc;
                article.IsTop = request.IsTop;
                detail.Content = request.Content;

                article.UpdateTime = DateTime.Now;
                article.Operator = userName;
                detail.UpdateTime = DateTime.Now;
                detail.Operator = userName;

                await _work.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"编辑文章异常;method={nameof(AdminEditArticleAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"编辑文章异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 后台-删除文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult> AdminDeleteArticleAsync(AdminDeleteArticleRequest request,long userId,string userName)
        {
            var response = new ApiResult();
            try
            {
                var article = await _articleRepository.Table
                    .FirstOrDefaultAsync(item => item.Id == request.ArticleId && item.Status == 1 && item.UserId == userId);
                var detail = await _articleDetailRepository.Table
                    .FirstOrDefaultAsync(item => item.ArticleId == request.ArticleId && item.Status == 1);
                if (article == null || detail == null)
                {
                    response.Code = Code.Error;
                    response.Message = "查无文章";
                    return response;
                }

                article.Status = 0;
                article.UpdateTime = DateTime.Now;
                article.Operator = userName;
                detail.Status = 0;
                detail.UpdateTime = DateTime.Now;
                detail.Operator = userName;

                await _work.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"删除文章异常;method={nameof(AdminDeleteArticleAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"删除文章异常：{ex.Message}";
                return response;
            }
        }
    }
}
