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

using System.Threading;
using System.Threading.Tasks;
using Mango.Core.ApiResponse;
using Mango.Service.Blog.Domain.AggregateModel.ArticleDataAggregate;
using Mango.Service.Infrastructure.Helper;
using MediatR;

namespace Mango.Service.Blog.Api.Application.Commands
{
    /// <summary>
    /// 增加文章阅读数命令Handler
    /// </summary>
    public class ArticleViewCommandHandler : ApiResultHepler, IRequestHandler<ArticleViewCommand,ApiResult>
    {
        private readonly IArticleDataRepository _articleDataRepository;

        public ArticleViewCommandHandler(IArticleDataRepository articleDataRepository)
        {
            _articleDataRepository = articleDataRepository;
        }
        
        /// <summary>
        /// Handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApiResult> Handle(ArticleViewCommand request, CancellationToken cancellationToken)
        {
            var article = await _articleDataRepository.GetByIdAsync(request.ArticleId);
            if (article == null) return NotFound();
            article.IncView(1);
            await _articleDataRepository.UpdateAsync(article);
            //await _articleDataRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return Ok();
        }
    }
}