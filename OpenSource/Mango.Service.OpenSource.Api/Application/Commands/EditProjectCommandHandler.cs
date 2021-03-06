﻿using Mango.Core.ApiResponse;
using Mango.Service.Infrastructure.Helper;
using Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate;
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

using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mango.Service.OpenSource.Api.Application.Commands
{
    /// <summary>
    /// 编辑项目命令Handler
    /// </summary>
    public class EditProjectCommandHandler : ApiResultHepler, IRequestHandler<EditProjectCommand, ApiResult>
    {
        private readonly IProjectRepository _projectRepository;

        public EditProjectCommandHandler(
            IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        /// <summary>
        /// Handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApiResult> Handle(EditProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if(project == null)
            {
                return NotFound("查无项目");
            }

            project.EditProjectInfo(request.ProjectName, request.Desc, request.RepositoryUrl, request.Image, request.Readme, request.Platform, request.SortId);
            await _projectRepository.UnitOfWork.SaveChangesAsync();

            return Ok("操作成功");
        }
    }
}
