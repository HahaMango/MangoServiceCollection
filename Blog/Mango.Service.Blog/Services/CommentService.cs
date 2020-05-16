using Mango.Core.ApiResponse;
using Mango.Service.Blog.Abstractions.Models.Dto;
using Mango.Service.Blog.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Services
{
    /// <summary>
    /// 评论服务实现类
    /// </summary>
    public class CommentService : ICommentService
    {
        public Task<ApiResult> ArticleCommentAsync(CommentRequest request, long userId)
        {
            throw new NotImplementedException();
        }
    }
}
