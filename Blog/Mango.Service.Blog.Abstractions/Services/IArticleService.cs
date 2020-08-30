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
using Mango.Core.DataStructure;
using Mango.Service.Blog.Abstractions.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Abstractions.Services
{
    /// <summary>
    /// 文章服务接口
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult> AddArticleAsync(AddArticleRequest request, long userId);

        /// <summary>
        /// 查询文章分页数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<PageList<ArticlePageListResponse>>> QueryArticlePageAsync(ArticlePageRequest request);

        /// <summary>
        /// 查询文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<ArticleDetailResponse>> QueryArticleDetailAsync(ArticleDetailRequest request);

        /// <summary>
        /// 文章点赞
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult> ArticleLikeAsync(ArticleLikeRequest request);

        /// <summary>
        /// 递增阅读数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult> IncViewAsync(IncArticleViewRequest request);

        /// <summary>
        /// 后台-查询文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<AdminArticleDetailResponse>> QueryAdminArticleDetailAsync(AdminArticleDetailRequest request);

        /// <summary>
        /// 后台-编辑文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult> AdminEditArticleAsync(AdminEditArticleRequest request,long userId, string userName);

        /// <summary>
        /// 后台-删除文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult> AdminDeleteArticleAsync(AdminDeleteArticleRequest request, long userId, string userName);
    }
}
