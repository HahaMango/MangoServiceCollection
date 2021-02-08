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

using Mango.EntityFramework.Abstractions;
using Mango.Service.Infrastructure.Persistence;

namespace Mango.Service.Blog.Domain.AggregateModel.ArticleDataAggregate
{
    /// <summary>
    /// 文章数据信息
    /// </summary>
    public class ArticleData:AggregateRoot
    {
        /// <summary>
        /// 观看数
        /// </summary>
        public int View { get; private set; }

        /// <summary>
        /// 观看数增量
        /// </summary>
        public int ViewInc { get; private set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int Comment { get; private set; }

        /// <summary>
        /// 评论数增量
        /// </summary>
        public int CommentInc { get; private set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int Like { get; private set; }

        /// <summary>
        /// 点赞数增量
        /// </summary>
        public int LikeInc { get; private set; }

        public ArticleData(long articleId, int view, int comment, int like)
        {
            Id = articleId;
            View = view;
            Comment = comment;
            Like = like;
        }

        public void IncView(int inc)
        {
            View += inc;
            ViewInc = inc;
        }

        public void IncLike(int inc)
        {
            Like += inc;
            LikeInc = inc;
        }

        public void IncComment(int inc)
        {
            Comment += inc;
            CommentInc = inc;
        }
    }
}