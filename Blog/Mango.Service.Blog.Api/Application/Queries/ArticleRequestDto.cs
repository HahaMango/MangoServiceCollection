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

using Mango.Core.DataStructure;

namespace Mango.Service.Blog.Api.Application.Queries
{
    public class ArticleRequestDto
    {
        
    }

    /// <summary>
    /// 查询文章分页列表请求
    /// </summary>
    public class QueryArticlePageRequest
    {
        /// <summary>
        /// 分页参数
        /// </summary>
        public PageParm PageParm { get; set; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public long UserId { get; set; }
    }

    /// <summary>
    /// 查询文章详情请求
    /// </summary>
    public class QueryArticleDetailRequest
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public long ArticleId { get; set; }
    }
}