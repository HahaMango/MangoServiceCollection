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
using Mango.Service.Blog.Api.Application.Interceptor;
using Mango.Service.Blog.Domain.Service;
using Mango.Service.Infrastructure.Helper;
using MediatR;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Api.Application.Commands
{
    /// <summary>
    /// 创建文章
    /// </summary>
    public class CreateArticleCommandHandler : ApiResultHepler, IRequestHandler<CreateArticleCommand, ApiResult>
    {
        private readonly ArticleDomainService _articleService;

        public CreateArticleCommandHandler(
            ArticleDomainService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// Handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [TransactionInterceptor]
        public async Task<ApiResult> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _articleService.CreateArticleNoCategory(request.UserId, request.Title, request.Desc, request.Content);
            if(article == null)
            {
                return Fail();
            }

            return Ok();
        }
    }
}
