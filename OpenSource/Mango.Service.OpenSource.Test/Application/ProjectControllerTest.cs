using Mango.Service.OpenSource.Api.Application.Commands;
using Mango.Service.OpenSource.Api.Application.Queries;
using Mango.Service.OpenSource.Controllers;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mango.Service.OpenSource.Test.Application
{
    public class ProjectControllerTest
    {
        private readonly Mock<ILogger<ProjectController>> _log;
        private readonly Mock<IMediator> _mediator;

        private readonly Mock<IProjectQueries> _projectQueries;

        public ProjectControllerTest()
        {
            _log = new Mock<ILogger<ProjectController>>();
            _mediator = new Mock<IMediator>();
            _projectQueries = new Mock<IProjectQueries>();
        }

        public async Task CreateProjectTest()
        {
            _mediator.Setup(m => m.Send(It.IsAny<CreateProjectCommand>(),new System.Threading.CancellationToken()))
                .Returns(Task.FromResult(new Core.ApiResponse.ApiResult
                {
                    Code = Core.Enums.Code.Ok
                }));

            var controller = new ProjectController(_mediator.Object, _log.Object, _projectQueries.Object);

            //暂不可测，需要修改
        }
    }
}
