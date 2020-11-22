using Mango.EntityFramework.Abstractions;
using Mango.EntityFramework.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Domain.AggregateModel.ArticleAggregate
{
    public interface IArticleRepository : IAggregateRepository<Article, long>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
