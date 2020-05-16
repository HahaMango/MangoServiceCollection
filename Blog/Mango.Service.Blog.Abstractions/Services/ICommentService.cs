using Mango.Core.ApiResponse;
using Mango.Service.Blog.Abstractions.Models.Dto;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Abstractions.Services
{
    /// <summary>
    /// 评论服务接口
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// 文章评论
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId">当前评论用户的Id,如果未登陆为0</param>
        /// <returns></returns>
        Task<ApiResult> ArticleCommentAsync(CommentRequest request, long userId);
    }
}
