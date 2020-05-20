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

using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Abstractions.Models.Dto
{
    public class CommentResponseDto
    {
    }

    #region 文章评论分页相应类

    public class CommentPageResponse
    {
        /// <summary>
        /// 评论Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 文章Id
        /// </summary>
        public long ArticleId { get; set; }

        /// <summary>
        /// 评论用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 评论用户Id
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 回复的评论
        /// </summary>
        public List<CommentPageResponse> ReplyComments { get; set; }

        /// <summary>
        /// 评论点赞数
        /// </summary>
        public int Like { get; set; }

        /// <summary>
        /// 回复数
        /// </summary>
        public int Reply { get; set; }
    }

    #endregion
}
