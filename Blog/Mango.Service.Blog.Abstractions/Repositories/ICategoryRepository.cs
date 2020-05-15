using Mango.EntityFramework.Abstractions.Repositories;
using Mango.Service.Blog.Abstractions.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Abstractions.Repositories
{
    /// <summary>
    /// 文章分类仓储接口
    /// </summary>
    public interface ICategoryRepository : IEfRepository<BlogDbContext,Category>
    {
    }
}
