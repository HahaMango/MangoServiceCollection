using Mango.EntityFramework.Abstractions.Repositories;
using Mango.EntityFramework.Repositories;
using Mango.Service.Blog.Abstractions.Models.Entities;
using Mango.Service.Blog.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Repositories
{
    /// <summary>
    /// 文章详情仓储实现
    /// </summary>
    public class ArticleDetailRepository : EfRepository<BlogDbContext,ArticleDetail>,IEfRepository<BlogDbContext,ArticleDetail>
    {
        public ArticleDetailRepository(BlogDbContext context) : base(context)
        {

        }
    }
}
