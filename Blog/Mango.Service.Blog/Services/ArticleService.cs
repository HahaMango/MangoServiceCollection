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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Core.ApiResponse;
using Mango.Core.DataStructure;
using Mango.Service.Blog.Abstractions.Models.Dto;
using Mango.Service.Blog.Abstractions.Services;

namespace Mango.Service.Blog.Services
{
    /// <summary>
    /// 文章服务实现类
    /// </summary>
    public class ArticleService : IArticleService
    {
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<ApiResult> AddArticleAsync(AddArticleRequest request, long userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 点赞文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult> ArticleLikeAsync(ArticleLikeRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<ArticleDetailResponse>> QueryArticleDetailAsync(ArticleDetailRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询文章分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<PageList<ArticlePageListResponse>>> QueryArticlePageAsync(ArticlePageRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
