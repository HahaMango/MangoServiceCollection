using System.ComponentModel.DataAnnotations;
using Mango.Core.ApiResponse;
using MediatR;

namespace Mango.Service.Blog.Api.Application.Commands
{
    public class DeleteArticleCommand: IRequest<ApiResult>
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        [Required(ErrorMessage = "文章ID不能为空")]
        public long ArticleId { get; set; }
    }
}