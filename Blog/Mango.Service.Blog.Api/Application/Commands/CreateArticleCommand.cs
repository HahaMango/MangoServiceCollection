using Mango.Core.ApiResponse;
using Mango.Service.Infrastructure.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Api.Application.Commands
{
    public class CreateArticleCommand : ITryCatchRequest<ApiResult>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

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
    }
}
