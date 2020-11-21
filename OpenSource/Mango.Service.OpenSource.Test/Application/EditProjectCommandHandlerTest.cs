using Mango.Core.Enums;
using Mango.Service.OpenSource.Api.Application.Commands;
using Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mango.Service.OpenSource.Test.Application
{
    public class EditProjectCommandHandlerTest
    {
        private readonly Mock<IProjectRepository> _projectRepository;

        public EditProjectCommandHandlerTest()
        {
            _projectRepository = new Mock<IProjectRepository>();
        }

        [Fact]
        public async Task EditProjectTest()
        {
            _projectRepository.Setup(p => p.GetByIdAsync(It.Is<long>(i => i == 123)))
                .Returns(Task.FromResult(new Project(12334113, "测试", "测试描述", null, null, null, "github")));

            _projectRepository.Setup(p => p.UnitOfWork.SaveChangesAsync(default))
                .Returns(Task.FromResult(1));

            var command = new EditProjectCommand
            {
                Id = 123,
                ProjectName = "修改标题1",
                Desc = "修改描述",
                Platform = "gitlab"
            };

            var handler = new EditProjectCommandHandler(_projectRepository.Object);
            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(Code.Ok, result.Code);
        }

        [Fact]
        public async Task EditProjectNotFoundTest()
        {
            _projectRepository.Setup(p => p.GetByIdAsync(It.Is<long>(i => i == 1234)))
                .Returns(Task.FromResult(new Project(12334113, "测试", "测试描述", null, null, null, "github")));

            _projectRepository.Setup(p => p.UnitOfWork.SaveChangesAsync(default))
                .Returns(Task.FromResult(1));

            var command = new EditProjectCommand
            {
                Id = 123,
                ProjectName = "修改标题1",
                Desc = "修改描述",
                Platform = "gitlab"
            };

            var handler = new EditProjectCommandHandler(_projectRepository.Object);
            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(Code.NoFound, result.Code);
        }
    }
}
