﻿/*--------------------------------------------------------------------------
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

using Mango.Core.ApiResponse;
using Mango.Core.ControllerAbstractions;
using Mango.Service.Blog.Api.Application.Commands;
using Mango.Service.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Api.Controllers
{
    [Route("api/blog/article")]
    public class ArticleController: MangoBaseApiController
    {
        private readonly ILogger<ArticleController> _log;
        private readonly IMediator _mediator;
        private readonly IAuthenticationService _authorizationService;

        public ArticleController(
            ILogger<ArticleController> log,
            IMediator mediator,
            IAuthenticationService authenticationService)
        {
            _log = log;
            _mediator = mediator;
            _authorizationService = authenticationService;
        }

        /// <summary>
        /// 创建文章
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Policy = "admin")]
        public async Task<ApiResult> CreateArticleAsync([FromBody]CreateArticleCommand command)
        {
            if (!ModelState.IsValid)
            {
                return InValidModelsError();
            }

            var user = _authorizationService.GetUser();
            if (user == null)
            {
                return AuthorizeError();
            }

            _log.LogInformation("执行CreateArticleAsync...控制器方法");

            return await _mediator.Send(command);
        }

        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("edit")]
        [Authorize(Policy = "admin")]
        public async Task<ApiResult> EditArticleAsync([FromBody] EditArticleCommand command)
        {
            if (!ModelState.IsValid)
            {
                return InValidModelsError();
            }

            var user = _authorizationService.GetUser();
            if (user == null)
            {
                return AuthorizeError();
            }

            _log.LogInformation("执行EditArticleAsync...控制器方法");

            return await _mediator.Send(command);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("delete")]
        [Authorize(Policy =  "admin")]
        public async Task<ApiResult> DeleteArticleAsync([FromBody] DeleteArticleCommand command)
        {
            if (!ModelState.IsValid)
            {
                return InValidModelsError();
            }
            
            _log.LogInformation("执行DeleteArticleAsync...控制器方法");

            return await _mediator.Send(command);
        }
    }
}
