using Mango.Core.ApiResponse;
using Mango.Core.Enums;
using Mango.Core.Serialization.Extension;
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
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mango.Service.OpenSource.Api.Application.Commands
{
    /// <summary>
    /// 编辑项目命令Handler
    /// </summary>
    public class EditProjectCommandHandler : IRequestHandler<EditProjectCommand, ApiResult>
    {
        private readonly ILogger<EditProjectCommandHandler> _log;
        private readonly IProjectRepository _projectRepository;

        public EditProjectCommandHandler(ILogger<EditProjectCommandHandler> log,
            IProjectRepository projectRepository)
        {
            _log = log;
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
            var response = new ApiResult();
            try
            {
                var project = await _projectRepository.GetByIdAsync(request.Id);
                project.EditProjectInfo(request.ProjectName, request.Desc, request.RepositoryUrl, request.Image, request.Readme, request.Platform);
                await _projectRepository.UnitOfWork.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch (Exception ex)
            {
                _log.LogError($"编辑项目异常;method={nameof(EditProjectCommandHandler)};param={request?.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"编辑项目异常：{ex.Message}";
                return response;
            }
        }
    }
}
