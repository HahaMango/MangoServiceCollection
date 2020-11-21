using Mango.Core.Enums;
using Mango.Service.Infrastructure.Services;
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
        private readonly Mock<IAuthenticationService> _authorizationService;
        private readonly Mock<IProjectQueries> _projectQueries;

        public ProjectControllerTest()
        {
            _log = new Mock<ILogger<ProjectController>>();
            _mediator = new Mock<IMediator>();
            _projectQueries = new Mock<IProjectQueries>();
            _authorizationService = new Mock<IAuthenticationService>();
        }

        [Fact]
        public async Task CreateProjectTest()
        {
            _mediator.Setup(m => m.Send(It.IsAny<CreateProjectCommand>(), new System.Threading.CancellationToken()))
                .Returns(Task.FromResult(new Core.ApiResponse.ApiResult
                {
                    Code = Core.Enums.Code.Ok
                }));

            _authorizationService.Setup(g => g.GetUser())
                .Returns(new Core.DataStructure.ControllerUser
                {
                    UserName = "测试用户",
                    UserId = 1234
                });

            var command = new CreateProjectCommand
            {
                ProjectName = "测试标题",
                Desc = "测试描述",
                Platform = "github"
            };

            var controller = new ProjectController(_mediator.Object, _log.Object, _authorizationService.Object, _projectQueries.Object);
            var result = await controller.CreateProjectAsync(command);

            Assert.NotNull(result);
            Assert.Equal(Code.Ok, result.Code);
        }

        [Fact]
        public async Task CreateProjectNotAuthTest()
        {
            _mediator.Setup(m => m.Send(It.IsAny<CreateProjectCommand>(), new System.Threading.CancellationToken()))
                .Returns(Task.FromResult(new Core.ApiResponse.ApiResult
                {
                    Code = Code.Ok
                }));

            _authorizationService.Setup(g => g.GetUser())
                .Returns(() => null);

            var command = new CreateProjectCommand
            {
                ProjectName = "测试标题",
                Desc = "测试描述",
                Platform = "github"
            };

            var controller = new ProjectController(_mediator.Object, _log.Object, _authorizationService.Object, _projectQueries.Object);
            var result = await controller.CreateProjectAsync(command);

            Assert.NotNull(result);
            Assert.Equal(Code.Unauthorized, result.Code);
        }

        [Fact]
        public async Task EditProjectTest()
        {
            _mediator.Setup(m => m.Send(It.IsAny<EditProjectCommand>(), new System.Threading.CancellationToken()))
                .Returns(Task.FromResult(new Core.ApiResponse.ApiResult
                {
                    Code = Code.Ok
                }));

            _authorizationService.Setup(g => g.GetUser())
                .Returns(new Core.DataStructure.ControllerUser
                {
                    UserName = "测试用户",
                    UserId = 1234
                });

            var command = new EditProjectCommand
            {
                Id = 1,
                ProjectName = "测试标题",
                Desc = "测试描述",
                Platform = "github"
            };

            var controller = new ProjectController(_mediator.Object, _log.Object, _authorizationService.Object, _projectQueries.Object);
            var result = await controller.EditProjectAsync(command);

            Assert.NotNull(result);
            Assert.Equal(Code.Ok, result.Code);
        }

        [Fact]
        public async Task EditProjectNotAuthTest()
        {
            _mediator.Setup(m => m.Send(It.IsAny<EditProjectCommand>(), new System.Threading.CancellationToken()))
                .Returns(Task.FromResult(new Core.ApiResponse.ApiResult
                {
                    Code = Code.Ok
                }));

            _authorizationService.Setup(g => g.GetUser())
                .Returns(() => null);

            var command = new EditProjectCommand
            {
                Id = 1,
                ProjectName = "测试标题",
                Desc = "测试描述",
                Platform = "github"
            };

            var controller = new ProjectController(_mediator.Object, _log.Object, _authorizationService.Object, _projectQueries.Object);
            var result = await controller.EditProjectAsync(command);

            Assert.NotNull(result);
            Assert.Equal(Code.Unauthorized, result.Code);
        }

        [Fact]
        public async Task DeleteProjectTest()
        {
            _mediator.Setup(m => m.Send(It.IsAny<DeleteProjectCommand>(), new System.Threading.CancellationToken()))
                .Returns(Task.FromResult(new Core.ApiResponse.ApiResult
                {
                    Code = Code.Ok
                }));

            _authorizationService.Setup(g => g.GetUser())
                .Returns(new Core.DataStructure.ControllerUser
                {
                    UserName = "测试用户",
                    UserId = 1234
                });

            var command = new DeleteProjectCommand
            {
                Id = 1,
            };

            var controller = new ProjectController(_mediator.Object, _log.Object, _authorizationService.Object, _projectQueries.Object);
            var result = await controller.DeleteProjectAsync(command);

            Assert.NotNull(result);
            Assert.Equal(Code.Ok, result.Code);
        }

        [Fact]
        public async Task DeleteProjectNotAuthTest()
        {
            _mediator.Setup(m => m.Send(It.IsAny<DeleteProjectCommand>(), new System.Threading.CancellationToken()))
                .Returns(Task.FromResult(new Core.ApiResponse.ApiResult
                {
                    Code = Code.Ok
                }));

            _authorizationService.Setup(g => g.GetUser())
                .Returns(() => null);

            var command = new DeleteProjectCommand
            {
                Id = 1,
            };

            var controller = new ProjectController(_mediator.Object, _log.Object, _authorizationService.Object, _projectQueries.Object);
            var result = await controller.DeleteProjectAsync(command);

            Assert.NotNull(result);
            Assert.Equal(Code.Unauthorized, result.Code);
        }

        [Fact]
        public async Task QueryProjectPageTest()
        {
            _projectQueries.Setup(q => q.QueryProjectPageAsync(It.IsAny<QueryProjectPageRequestDto>(), default))
                .Returns(Task.FromResult(new Core.DataStructure.PageList<QueryProjectResponseDto>
                {
                    
                }));

            var request = new QueryProjectPageRequestDto
            {
                
            };

            var controller = new ProjectController(_mediator.Object, _log.Object, _authorizationService.Object, _projectQueries.Object);
            var result = await controller.QueryProjectPageAsync(request,default);

            Assert.NotNull(result);
            Assert.Equal(Code.Ok, result.Code);
        }

        [Fact]
        public async Task QueryProjectDetailTest()
        {
            _projectQueries.Setup(q => q.QueryProjectDetailAsync(It.IsAny<long>(), default))
                .Returns(Task.FromResult(new QueryProjectResponseDto
                {
                }));

            var controller = new ProjectController(_mediator.Object, _log.Object, _authorizationService.Object, _projectQueries.Object);
            var result = await controller.QueryProjectDetailAsync(123,default);

            Assert.NotNull(result);
            Assert.Equal(Code.Ok, result.Code);
        }
    }
}
