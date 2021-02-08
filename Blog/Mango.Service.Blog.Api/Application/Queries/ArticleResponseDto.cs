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
using System.Collections.Generic;

namespace Mango.Service.Blog.Api.Application.Queries
{
    public class ArticleResponseDto
    {
        
    }
    
    /// <summary>
    /// 查询文章分页列表响应
    /// </summary>
    public class QueryArticlePageResponse
    {
        /// <summary>
        /// 文章id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 分类id
        /// </summary>
        public long CategoryId { get; set; }
        
        /// <summary>
        /// 分类名称
        /// </summary>
        public List<QueryCategoryResponse> CategoryName { get; set; }

        /// <summary>
        /// 文章描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 阅读数
        /// </summary>
        public int View { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int Comment { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int Like { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public int IsTop { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 查询文章详情响应
    /// </summary>
    public class QueryArticleDetailResponse
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 作者Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 作者昵称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 文章分类Id
        /// </summary>
        public long CategoryId { get; set; }
        
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 阅读数
        /// </summary>
        public int View { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int Comment { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int Like { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}