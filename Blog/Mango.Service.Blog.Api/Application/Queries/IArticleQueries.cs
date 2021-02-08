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

using System.Threading;
using System.Threading.Tasks;
using Mango.Core.DataStructure;

namespace Mango.Service.Blog.Api.Application.Queries
{
    /// <summary>
    /// 文章查询接口（职责分离模式）
    /// </summary>
    public interface IArticleQueries
    {
        /// <summary>
        /// 查询文章分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PageList<QueryArticlePageResponse>> QueryArticlePageAsync(QueryArticlePageRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 查询文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<QueryArticleDetailResponse> QueryArticleDetailAsync(QueryArticleDetailRequest request,
            CancellationToken cancellationToken = default);
    }
}