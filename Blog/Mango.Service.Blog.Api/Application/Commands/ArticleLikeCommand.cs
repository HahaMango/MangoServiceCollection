/*--------------------------------------------------------------------------
//
//  Copyright 2021 Chiva Chen
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

using System.ComponentModel.DataAnnotations;
using Mango.Core.ApiResponse;
using Mango.Service.Infrastructure.ModelValidation;
using MediatR;

namespace Mango.Service.Blog.Api.Application.Commands
{
    /// <summary>
    /// 文章点赞命令
    /// </summary>
    public class ArticleLikeCommand : IRequest<ApiResult>
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        [Required(ErrorMessage = "文章Id不能为空")]
        public long ArticleId { get; set; }

        /// <summary>
        /// -1：取消点赞 1：点赞
        /// </summary>
        [IsIn(-1,1,ErrorMessage = "参数不正确")]
        public int State { get; set; }
    }
}