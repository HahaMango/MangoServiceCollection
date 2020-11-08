/*--------------------------------------------------------------------------
//
//  Copyright 2020 Chiva Chen
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//
/*--------------------------------------------------------------------------*/

using Mango.Core.ApiResponse;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Mango.Service.OpenSource.Api.Application.Commands
{
    public class EditProjectCommand : IRequest<ApiResult>
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        [Required(ErrorMessage = "项目ID不能为空")]
        public long Id { get; set; }

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
