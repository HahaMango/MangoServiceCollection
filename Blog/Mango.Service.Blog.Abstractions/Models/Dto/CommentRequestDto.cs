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
    public class CommentRequestDto
    {
    }


    #region 文章评论请求类

    public class CommentRequest
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public long ArticleId { get; set; }

        /// <summary>
        /// 评论显示用户名（匿名用户需要传）
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
    }

    #endregion

    #region 回复评论请求类
    public class CommentReplyRequest
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public long ArticleId { get; set; }

        /// <summary>
        /// 评论显示用户名（匿名用户需要传）
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 回复的评论Id
        /// </summary>
        public long ReplyCommentId { get; set; }

        /// <summary>
        /// 回复的子评论Id（回复子评论时要传）
        /// </summary>
        public long? ReplySubCommentId { get; set; }

        /// <summary>
        /// 回复的子评论用户Id（回复子评论时要传）
        /// </summary>
        public long? ReplySubUserId { get; set; }

        /// <summary>
        /// 回复的子评论的用户名（匿名用户时显示此名称）
        /// </summary>
        public string ReplySubUserName { get; set; }
    }
    #endregion

    #region 查询文章评论分页请求类

    public class CommentPageRequest
    {
        /// <summary>
        /// 分页参数
        /// </summary>
        public PageParm PageParm { get; set; }

        /// <summary>
        /// 文章Id
        /// </summary>
        public long articleId { get; set; }
    }

    #endregion
}
