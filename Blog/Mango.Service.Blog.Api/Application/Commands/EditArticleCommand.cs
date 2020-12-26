using Mango.Core.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Api.Application.Commands
{
    public class EditArticleCommand : IRequest<ApiResult>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 文章ID
        /// </summary>
        [Required(ErrorMessage = "文章ID不能为空")]
        public long ArticleId { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        [Required(ErrorMessage = "文章标题不能为空")]
        [MinLength(1)]
        public string Title { get; set; }

        /// <summary>
        /// 文章描述
        /// </summary>
        [Required(ErrorMessage = "文章描述不能为空")]
        [MinLength(10)]
        public string Desc { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        [Required(ErrorMessage = "文章内容不能为空")]
        public string Content { get; set; }
        
        /// <summary>
        /// 文章分类ID
        /// </summary>
        public List<string> CategoryIds { get; set; }
    }
}
