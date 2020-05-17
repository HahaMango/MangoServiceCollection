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

using Mango.EntityFramework.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mango.Service.Blog.Abstractions.Models.Entities
{
    /// <summary>
    /// 文章表
    /// </summary>
    [Table("article")]
    public class Article : SnowFlakeEntity
    {
        /// <summary>
        /// 作者Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 文章分类Id
        /// </summary>
        public long CategoryId { get; set; }

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

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 状态 0：删除 1：正常
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Article() { }

        /// <summary>
        /// 设置Id
        /// </summary>
        /// <param name="setId"></param>
        public Article(bool setId)
        {
            base.SetId();
        }
    }
}
