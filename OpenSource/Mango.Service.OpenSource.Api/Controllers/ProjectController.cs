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
using Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public ProjectController(
            IMediator mediator,
            ILogger<ProjectController> log)
        {
            _mediator = mediator;
            _log = log;
        }

        [HttpPost("create")]
        public async Task<ApiResult> CreateProjectAsync([FromBody]CreateProjectCommand command)
        {
            if (!ModelState.IsValid)
            {
                return InValidModelsError();
            }

            _log.LogInformation("执行CreateProjectAsync...控制器方法");

            return await _mediator.Send(command);
        }
    }
}