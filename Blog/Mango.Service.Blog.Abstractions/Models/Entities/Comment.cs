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

using Mango.Core.EntityFramework.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mango.Service.Blog.Abstractions.Models.Entities
{
    /// <summary>
    /// 评论表
    /// </summary>
    [Table("comment")]
    public class Comment:SnowFlakeEntity
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public long ArticleId { get; set; }

        /// <summary>
        /// 评论用户Id，游客评论没有Id
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 评论用户Id
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否为回复评论
        /// </summary>
        public int IsReply { get; set; }

        /// <summary>
        /// 是否子评论回复 0：子评论 1：子评论回复
        /// </summary>
        public int IsSubReply { get; set; }

        /// <summary>
        /// 回复的子评论Id
        /// </summary>
        public long? ReplySubCommentId { get; set; }

        /// <summary>
        /// 回复的子评论用户Id
        /// </summary>
        public long? ReplySubUserId { get; set; }

        /// <summary>
        /// 回复的子评论用户名
        /// </summary>
        public string ReplySubUserName { get; set; }

        /// <summary>
        /// 回复的评论Id
        /// </summary>
        public long? ReplyCommentId { get; set; }

        /// <summary>
        /// 状态 0：删除 1：正常
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 评论点赞数
        /// </summary>
        public int Like { get; set; }

        /// <summary>
        /// 回复数
        /// </summary>
        public int Reply { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新评论时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Comment() { }

        /// <summary>
        /// 设置Id
        /// </summary>
        /// <param name="set"></param>
        public Comment(bool set)
        {
            base.SetId();
        }
    }
}
