using Mango.EntityFramework.Abstractions.Repositories;
using Mango.Service.Blog.Abstractions.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Abstractions.Repositories
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public interface IUserRepository : IEfRepository<BlogDbContext,User>
    {
    }
}
