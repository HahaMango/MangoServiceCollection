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
using Mango.Core.ControllerAbstractions;
using Mango.Core.DataStructure;
using Mango.Service.OpenSource.Api.Application.Commands;
using Mango.Service.OpenSource.Api.Application.Queries;
using Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Mango.Service.OpenSource.Controllers
{
    /// <summary>
    /// 文章Controller
    /// </summary>
    [Route("api/opensource/project")]
    public class ProjectController : MangoUserApiController
    {
        private readonly ILogger<ProjectController> _log;
        private readonly IMediator _mediator;

        private readonly IProjectQueries _projectQueries;

        public ProjectController(
            IMediator mediator,
            ILogger<ProjectController> log,
            IProjectQueries projectQueries)
        {
            _mediator = mediator;
            _log = log;
            _projectQueries = projectQueries;
        }

        /// <summary>
        /// 创建项目
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Policy = "admin")]
        public async Task<ApiResult> CreateProjectAsync([FromBody]CreateProjectCommand command)
        {
            if (!ModelState.IsValid)
            {
                return InValidModelsError();
            }
            var user = GetUser();
            if(user == null)
            {
                return AuthorizeError();
            }
            command.UserId = user.UserId;

            _log.LogInformation("执行CreateProjectAsync...控制器方法");

            return await _mediator.Send(command);
        }

        /// <summary>
        /// 编辑项目
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("edit")]
        [Authorize(Policy = "admin")]
        public async Task<ApiResult> EditProjectAsync([FromBody]EditProjectCommand command)
        {
            if (!ModelState.IsValid)
            {
                return InValidModelsError();
            }
            var user = GetUser();
            if (user == null)
            {
                return AuthorizeError();
            }

            _log.LogInformation("执行EditProjectAsync...控制器方法");

            return await _mediator.Send(command);
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("delete")]
        [Authorize(Policy = "admin")]
        public async Task<ApiResult> DeleteProjectAsync([FromBody]DeleteProjectCommand command)
        {
            if (!ModelState.IsValid)
            {
                return InValidModelsError();
            }
            var user = GetUser();
            if (user == null)
            {
                return AuthorizeError();
            }

            _log.LogInformation("执行DeleteProjectAsync...控制器方法");

            return await _mediator.Send(command);
        }

        /// <summary>
        /// 查询开源项目分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<ApiResult<PageList<QueryProjectResponseDto>>> QueryProjectPageAsync([FromBody]QueryProjectPageRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return InValidModelsError<PageList<QueryProjectResponseDto>>();
            }

            var response = new ApiResult<PageList<QueryProjectResponseDto>>();
            try
            {
                _log.LogInformation("执行QueryProjectPageAsync...控制器方法");
                var result = await _projectQueries.QueryProjectPageAsync(request);
                response.Code = Core.Enums.Code.Ok;
                response.Message = "查询成功";
                response.Data = result;
                return response;
            }
            catch(Exception ex)
            {
                _log.LogError($"执行QueryProjectPageAsync异常,{ex.Message}");
                response.Code = Core.Enums.Code.Error;
                response.Message = "查询异常";
                return response;
            }
        }

        /// <summary>
        /// 查询开源项目详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail")]
        public async Task<ApiResult<QueryProjectResponseDto>> QueryProjectDetailAsync(long id)
        {
            var response = new ApiResult<QueryProjectResponseDto>();
            try
            {
                _log.LogInformation("执行QueryProjectDetailAsync...控制器方法");
                var result = await _projectQueries.QueryProjectDetailAsync(id);
                response.Code = Core.Enums.Code.Ok;
                response.Message = "查询成功";
                response.Data = result;
                return response;
            }
            catch (Exception ex)
            {
                _log.LogError($"执行QueryProjectDetailAsync异常,{ex.Message}");
                response.Code = Core.Enums.Code.Error;
                response.Message = "查询异常";
                return response;
            }
        }
    }
}