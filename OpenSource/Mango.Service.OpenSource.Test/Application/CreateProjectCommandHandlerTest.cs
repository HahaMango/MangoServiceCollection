using Mango.Core.Enums;
using Mango.Service.OpenSource.Api.Application.Commands;
using Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Mango.Service.OpenSource.Test.Application
{
    public class CreateProjectCommandHandlerTest
    {
        private readonly Mock<IProjectRepository> _projectRepository;

        public CreateProjectCommandHandlerTest()
        {
            _projectRepository = new Mock<IProjectRepository>();
        }

        [Fact]
        public async Task CreateProjectTest()
        {
            _projectRepository.Setup(p => p.AddAsync(It.IsAny<Project>()))
                .Returns(Task.CompletedTask);

            _projectRepository.Setup(p => p.UnitOfWork.SaveChangesAsync(default))
                .Returns(Task.FromResult(1));

            var command = new CreateProjectCommand
            {
                UserId = 123,
                ProjectName = "测试",
                Desc= "测试描述",
                Platform = "github"
            };

            var handler = new CreateProjectCommandHandler(_projectRepository.Object);
            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(Code.Ok, result.Code);
        }
    }
}
