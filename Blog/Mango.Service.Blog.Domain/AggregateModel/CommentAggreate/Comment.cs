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

using Mango.Service.Blog.Domain.AggregateModel.Enum;
using Mango.Service.Blog.Domain.Event;
using Mango.Service.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Domain.AggregateModel.CommentAggreate
{
    /// <summary>
    /// 文章评论聚合
    /// </summary>
    public class Comment: AggregateRoot
    {
        private long _articleId;

        public EntityStatusEnum Status { get; private set; }

        /// <summary>
        /// 评论用户信息
        /// </summary>
        public BloggerInfo BloggerInfo { get; private set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// 是否子评论 0：否 1：是
        /// </summary>
        public int IsSubComment { get; private set; }

        /// <summary>
        /// 回复的主评论Id
        /// </summary>
        public long? ReplyMainCommentId { get; private set; }

        /// <summary>
        /// 回复的子评论Id
        /// </summary>
        public long? ReplySubCommentId { get; private set; }

        /// <summary>
        /// 回复的评论的用户信息
        /// </summary>
        public BloggerInfo ReplySubBloggerInfo { get; private set; }

        private int _like;

        private int _reply;

        private DateTime _createTime;

        private DateTime? _updateTime;

        protected Comment() { }

        /// <summary>
        /// 创建评论
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="articleId"></param>
        /// <param name="content"></param>
        /// <param name="replyComment">回复的评论</param>
        public Comment(long? userId, string userName ,long articleId,string content, Comment replyComment = null)
        {
            if (string.IsNullOrEmpty(content)) throw new ArgumentNullException(nameof(content));

            SetId();
            BloggerInfo = new BloggerInfo(userId, userName);
            _articleId = articleId;
            Content = content;
            Status = EntityStatusEnum.Available;
            ReplyComment(replyComment);
            _createTime = DateTime.Now;

            AddDomainEvent(new CreateCommentEvent(BloggerInfo.UserId, _articleId));
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        public void Delete()
        {
            Status = EntityStatusEnum.Deleted;
        }

        /// <summary>
        /// 回复评论
        /// </summary>
        /// <param name="comment"></param>
        private void ReplyComment(Comment comment)
        {
            if (comment == null) return;

            IsSubComment = 1;
            if(comment.IsSubComment == 0)
            {
                //回复的是主评论
                ReplyMainCommentId = comment.Id;
            }
            else
            {
                //回复的是子评论
                ReplyMainCommentId = comment.ReplyMainCommentId;
                ReplySubCommentId = comment.Id;
                ReplySubBloggerInfo = new BloggerInfo(comment.BloggerInfo.UserId, comment.BloggerInfo.UserName);
            }
        }
    }
}
