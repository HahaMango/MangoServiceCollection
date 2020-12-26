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
using Mango.Service.Blog.Infrastructure.Repositories;
using Mango.Service.Infrastructure.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mango.Service.Blog.Api.Application.Interceptor;
using Mango.Service.Blog.Domain.AggregateModel.ArticleAggregate;
using Mango.Service.Blog.Domain.AggregateModel.CategoryAggreate;

namespace Mango.Service.Blog.Api.Application.Commands
{
    /// <summary>
    /// 编辑文章Handler
    /// </summary>
    public class EditArticleCommandHandler : ApiResultHepler, IRequestHandler<EditArticleCommand, ApiResult>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;

        public EditArticleCommandHandler(IArticleRepository articleRepository,ICategoryRepository categoryRepository)
        {
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [TransactionInterceptor]
        public async Task<ApiResult> Handle(EditArticleCommand request, CancellationToken cancellationToken)
        {
            //获取文章
            var article = await _articleRepository.GetByIdAsync(request.ArticleId);
            if (article == null)
            {
                return NotFound();
            }
            //获取文章分类
            if (request.CategoryIds == null || !request.CategoryIds.Any())
            {
                return Fail("文章分类不能为空");
            }
            var categoriesIds = new long[request.CategoryIds.Count];
            for (var i = 0; i < request.CategoryIds.Count; i++)
            {
                categoriesIds[i] = Convert.ToInt64(request.CategoryIds[i]);
            }
            var categories = await _categoryRepository.GetByIdsAsync(categoriesIds);
            //编辑文章
            article.Edit(request.Title,request.Desc,request.Content,categories.ToList());

            await _articleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            return Ok();
        }
    }
}
