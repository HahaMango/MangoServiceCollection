using Mango.Service.Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Domain.Event
{
    public class CreateCommentEvent: IDomainEvent
    {
        /// <summary>
        /// 评论的博客用户（有可能匿名）
        /// </summary>
        public long? CommentBloggerId { get; private set; }

        public long ArticleId { get; private set; }

        public CreateCommentEvent(long? commentBloggerId, long articleId)
        {
            CommentBloggerId = commentBloggerId;
            ArticleId = articleId;
        }
    }
}
