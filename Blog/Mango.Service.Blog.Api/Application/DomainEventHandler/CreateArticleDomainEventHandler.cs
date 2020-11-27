using Mango.Service.Blog.Domain.AggregateModel.BloggerAggreate;
using Mango.Service.Blog.Domain.Event;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Api.Application.DomainEventHandler
{
    /// <summary>
    /// 创建文章领域事件
    /// </summary>
    public class CreateArticleDomainEventHandler : INotificationHandler<CreateArticleEvent>
    {
        private readonly IBloggerRepository _bloggerRepository;

        public CreateArticleDomainEventHandler(IBloggerRepository bloggerRepository)
        {
            _bloggerRepository = bloggerRepository;
        }

        public async Task Handle(CreateArticleEvent notification, CancellationToken cancellationToken)
        {
            var blogger = await _bloggerRepository.GetByIdAsync(notification.BloggerId);
            if(blogger != null)
            {
                blogger.ArticleCount++;
            }
        }
    }
}
