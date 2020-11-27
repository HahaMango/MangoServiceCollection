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

using Mango.Service.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Domain.AggregateModel.BloggerAggreate
{
    /// <summary>
    /// 博客用户信息聚合
    /// </summary>
    public class Blogger: AggregateRoot
    {
        /// <summary>
        /// 博客用户名称
        /// </summary>
        public string BloggerName { get; private set; }

        /// <summary>
        /// 发表的文章数
        /// </summary>
        public int ArticleCount { get; set; }

        /// <summary>
        /// 文章点赞数
        /// </summary>
        public int ArticleLikeCount { get; set; }

        /// <summary>
        /// 文章阅读数
        /// </summary>
        public int ArticleViewCount { get; set; }

        /// <summary>
        /// 阅读的文章数
        /// </summary>
        public int ViewCount { get; set; }

        private DateTime _createTime;

        private DateTime? _updateTime;

        protected Blogger()
        {

        }

        /// <summary>
        /// 创建博客用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        public Blogger(long userId,string userName)
        {
            ThrowIfNameEmpty(userName);

            Id = userId;
            BloggerName = userName;
            _createTime = DateTime.Now;
        }

        /// <summary>
        /// 编辑博客用户名
        /// </summary>
        /// <param name="bloggerName"></param>
        public void EditName(string bloggerName)
        {
            ThrowIfNameEmpty(bloggerName);

            BloggerName = bloggerName;
        }

        private void ThrowIfNameEmpty(string userName)
        {
            if(string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName));
        }
    }
}
