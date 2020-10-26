using Mango.Core.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.OpenSource.Api.Application.Commands
{
    /// <summary>
    /// 创建项目命令
    /// </summary>
    public class CreateProjectCommand : IRequest<ApiResult>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [Required(ErrorMessage = "项目名称不能为空")]
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目描述
        /// </summary>
        [Required(ErrorMessage = "项目描述不能为空")]
        public string Desc { get; set; }

        /// <summary>
        /// 项目URL
        /// </summary>
        public string RepositoryUrl { get; set; }

        /// <summary>
        /// 项目主图
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 项目readme
        /// </summary>
        public string Readme { get; set; }

        /// <summary>
        /// 项目平台
        /// </summary>
        [Required(ErrorMessage = "项目平台不能为空")]
        public string Platform { get; set; }
    }
}
