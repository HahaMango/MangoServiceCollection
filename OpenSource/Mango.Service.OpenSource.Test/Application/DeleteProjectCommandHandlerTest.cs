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
    public class DeleteProjectCommandHandlerTest
    {
        private readonly Mock<IProjectRepository> _projectRepository;

        public DeleteProjectCommandHandlerTest()
        {
            _projectRepository = new Mock<IProjectRepository>();
        }

        [Fact]
        public async Task DeleteProjectTest()
        {
            _projectRepository.Setup(p => p.GetByIdAsync(It.Is<long>(i => i == 123)))
                .Returns(Task.FromResult(new Project(12334113,"测试","测试描述",null,null,null,"github")));

            _projectRepository.Setup(p => p.UnitOfWork.SaveChangesAsync())
                .Returns(Task.FromResult(1));

            var command = new DeleteProjectCommand
            {
                Id = 123
            };

            var handler = new DeleteProjectCommandHandler(_projectRepository.Object);
            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(Code.Ok, result.Code);
        }

        [Fact]
        public async Task DeleteProjectNotFoundTest()
        {
            _projectRepository.Setup(p => p.GetByIdAsync(It.Is<long>(i => i == 123)))
                .Returns(Task.FromResult(new Project(12334113, "测试", "测试描述", null, null, null, "github")));

            _projectRepository.Setup(p => p.UnitOfWork.SaveChangesAsync())
                .Returns(Task.FromResult(1));

            var command = new DeleteProjectCommand
            {
                Id = 1234
            };

            var handler = new DeleteProjectCommandHandler(_projectRepository.Object);
            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(Code.NoFound, result.Code);
        }
    }
}
