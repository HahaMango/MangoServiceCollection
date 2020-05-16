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
    /// 用户仓储实现类
    /// </summary>
    public class UserRepository : EfRepository<BlogDbContext,User>,IEfRepository<BlogDbContext,User>
    {
        public UserRepository(BlogDbContext context) : base(context)
        {

        }
    }
}
