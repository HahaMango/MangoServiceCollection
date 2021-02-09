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

using System;
using Mango.Core.ApiResponse;
using Mango.Core.ControllerAbstractions;
using Mango.Service.Blog.Api.Application.Commands;
using Mango.Service.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Mango.Core.DataStructure;
using Mango.Core.Enums;
using Mango.Service.Blog.Api.Application.Queries;

namespace Mango.Service.Blog.Api.Controllers
{
    [Route("api/blog/article")]
    public class ArticleController: MangoBaseApiController
    {
        private readonly ILogger<ArticleController> _log;
        private readonly IMediator _mediator;
        private readonly IAuthenticationService _authorizationService;
        private readonly IArticleQueries _articleQueries;

        public ArticleController(
            ILogger<ArticleController> log,
            IMediator mediator,
            IAuthenticationService authenticationService,
            IArticleQueries articleQueries)
        {
            _log = log;
            _mediator = mediator;
            _authorizationService = authenticationService;
            _articleQueries = articleQueries;
        }

        #region 后台
        
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
        
        #endregion

        #region 前台

        /// <summary>
        /// 查询文章分页数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("page")]
        public async Task<ApiResult<PageList<QueryArticlePageResponse>>> QueryArticlePageAsync(QueryArticlePageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return InValidModelsError<PageList<QueryArticlePageResponse>>();
            }
            
            var response = new ApiResult<PageList<QueryArticlePageResponse>>();
            try
            {
                _log.LogInformation("执行QueryArticlePageAsync...控制器方法");
                var result = await _articleQueries.QueryArticlePageAsync(request, HttpContext.RequestAborted);
                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = result;
                return response;
            }
            catch (Exception ex)
            {
                _log.LogError($"执行QueryArticlePageAsync异常,{ex.Message}");
                response.Code = Core.Enums.Code.Error;
                response.Message = "查询异常";
                return response;
            }
        }

        /// <summary>
        /// 查询文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("detail")]
        public async Task<ApiResult<QueryArticleDetailResponse>> QueryArticleDetailAsync(QueryArticleDetailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return InValidModelsError<QueryArticleDetailResponse>();
            }

            var response = new ApiResult<QueryArticleDetailResponse>();
            try
            {
                _log.LogInformation("执行QueryArticleDetailAsync...控制器方法");
                var result = await _articleQueries.QueryArticleDetailAsync(request, HttpContext.RequestAborted);
                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = result;
                return response;
            }
            catch (Exception ex)
            {
                _log.LogError($"执行QueryArticleDetailAsync异常,{ex.Message}");
                response.Code = Core.Enums.Code.Error;
                response.Message = "查询异常";
                return response;
            }
        }

        /// <summary>
        /// 点赞文章
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("like")]
        public async Task<ApiResult> ArticleLikeAsync([FromBody] ArticleLikeCommand command)
        {
            if (!ModelState.IsValid)
            {
                return InValidModelsError();
            }
            
            _log.LogInformation("执行ArticleLikeAsync...控制器方法");
            return await _mediator.Send(command, HttpContext.RequestAborted);
        }

        [HttpPost("incview")]
        public async Task<ApiResult> ArticleIncViewAsync([FromBody] ArticleViewCommand command)
        {
            if (!ModelState.IsValid)
            {
                return InValidModelsError();
            }
            
            _log.LogInformation("执行ArticleIncViewAsync...控制器方法");
            return await _mediator.Send(command, HttpContext.RequestAborted);
        }
        
        #endregion
    }
}
