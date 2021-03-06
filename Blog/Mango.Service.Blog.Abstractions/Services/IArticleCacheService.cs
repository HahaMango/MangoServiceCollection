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

using Mango.Core.ApiResponse;
using Mango.Service.Blog.Abstractions.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Abstractions.Services
{
    /// <summary>
    /// 文章缓存服务，缓存如点赞，阅读数等操作
    /// </summary>
    public interface IArticleCacheService
    {

        /// <summary>
        /// 查询点赞数
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<int> QueryLikeAsync(long articleId);

        /// <summary>
        /// 查询阅读数
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<int> QueryViewAsync(long articleId);

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EditLikeAsync(ArticleLikeRequest request);

        /// <summary>
        /// 阅读数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task IncViewAsync(IncArticleViewRequest request);
    }
}
