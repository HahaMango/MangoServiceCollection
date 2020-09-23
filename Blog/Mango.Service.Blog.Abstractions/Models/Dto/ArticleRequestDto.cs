/*--------------------------------------------------------------------------
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

using Mango.Core.DataStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Abstractions.Models.Dto
{
    public class ArticleRequestDto
    {

    }

    #region 添加文章请求类

    public class AddArticleRequest
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 文章类目Id (为空使用默认分类)
        /// </summary>
        public long? CategoryId { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 文章内容格式类型 0：MD 目前仅支持MD
        /// </summary>
        public int ContentType { get; set; } 
    }

    #endregion

    #region 文章分页请求类

    public class ArticlePageRequest
    {
        /// <summary>
        /// 分页参数
        /// </summary>
        public PageParm PageParm { get; set; }

        /// <summary>
        /// 作者Id
        /// </summary>
        public long UserId { get;set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 作者名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 筛选开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 筛选结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    #endregion

    #region 文章详情请求类

    public class ArticleDetailRequest
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public long ArticleId { get; set; }
    }

    #endregion

    #region 文章点赞请求类

    public class ArticleLikeRequest
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public long ArticleId { get; set; }

        /// <summary>
        /// -1：取消点赞 1：点赞
        /// </summary>
        public int State { get; set; }
    }

    #endregion

    #region 文章浏览数增加请求

    public class IncArticleViewRequest
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public long ArticleId { get; set; }
    }

    #endregion

    #region 后台-查询文章详情
    public class AdminArticleDetailRequest
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public long ArticleId { get; set; }
    }
    #endregion

    #region 后台-编辑文章
    public class AdminEditArticleRequest
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public long ArticleId { get; set; }
        
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public int IsTop { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }
    }
    #endregion

    #region 后台-删除
    public class AdminDeleteArticleRequest
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public long ArticleId { get; set; }
    }
    #endregion
}
