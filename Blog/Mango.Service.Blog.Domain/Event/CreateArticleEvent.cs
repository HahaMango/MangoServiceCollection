using Mango.Service.Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Domain.Event
{
    public class CreateArticleEvent : IDomainEvent
    {
        public long BloggerId { get; private set; }

        public long ArticleId { get; private set; }

        public CreateArticleEvent(long bloggerId, long articleId)
        {
            BloggerId = bloggerId;
            ArticleId = articleId;
        }
    }
}
