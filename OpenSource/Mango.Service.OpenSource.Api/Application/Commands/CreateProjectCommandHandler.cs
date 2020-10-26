using Mango.Core.ApiResponse;
using Mango.Core.Enums;
using Mango.Core.Serialization.Extension;
using Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate;
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
    /// 创建项目命令Handler
    /// </summary>
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ApiResult>
    {
        private readonly ILogger<CreateProjectCommandHandler> _log;
        private readonly IProjectRepository _projectRepository;

        public CreateProjectCommandHandler(
            ILogger<CreateProjectCommandHandler> log,
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
        public async Task<ApiResult> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResult();
            try
            {
                var project = new Project(
                    request.UserId,
                    request.ProjectName,
                    request.Desc,
                    request.RepositoryUrl,
                    request.Image,
                    request.Readme,
                    request.Platform);
                await _projectRepository.AddAsync(project);
                await _projectRepository.UnitOfWork.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch(Exception ex)
            {
                _log.LogError($"创建项目异常;method={nameof(CreateProjectCommandHandler)};param={request?.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"创建项目异常：{ex.Message}";
                return response;
            }
        }
    }
}
