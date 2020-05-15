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
    /// 文章仓储接口实现
    /// </summary>
    public class ArticleRepository : EfRepository<BlogDbContext,Article> ,IEfRepository<BlogDbContext,Article>
    {
        public ArticleRepository(BlogDbContext context) : base(context)
        {
            
        }
    }
}
