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

using System.Threading;
using System.Threading.Tasks;
using Mango.Core.ApiResponse;
using Mango.Service.Blog.Api.Application.Interceptor;
using Mango.Service.Blog.Domain.AggregateModel.ArticleAggregate;
using Mango.Service.Infrastructure.Helper;
using MediatR;

namespace Mango.Service.Blog.Api.Application.Commands
{
    /// <summary>
    /// 删除文章
    /// </summary>
    public class DeleteArticleCommandHandler : ApiResultHepler,IRequestHandler<DeleteArticleCommand,ApiResult>
    {
        private readonly IArticleRepository _articleRepository;

        public DeleteArticleCommandHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        
        /// <summary>
        /// Handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [TransactionInterceptor]
        public async Task<ApiResult> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetByIdAsync(request.ArticleId);
            if (article == null)
            {
                return NotFound();
            }

            await _articleRepository.RemoveAsync(article);

            await _articleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            return Ok();
        }
    }
}