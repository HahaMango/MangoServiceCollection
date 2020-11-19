using Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate;
using Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate.Enum;
using Xunit;
using System;

namespace Mango.Service.OpenSource.Test.Domain
{
    public class ProjectTest
    {
        [Fact]
        public void CreateProjectTest()
        {
            var userId = 233;
            var projectName = "测试";
            var desc = "测试描述";
            var repositoryUrl = "https://hahamango.cn";
            var image = "https://";
            var readme = "testReadme";
            var platform = "github";

            var project = new Project(userId, projectName, desc, repositoryUrl, image, readme, platform);

            Assert.NotNull(project);
        }

        [Fact]
        public void CreateProjectNoProjectNameTest()
        {
            var userId = 233;
            var projectName = default(string);
            var desc = "测试描述";
            var repositoryUrl = "https://hahamango.cn";
            var image = "https://";
            var readme = "testReadme";
            var platform = "github";

            Assert.Throws<ArgumentNullException>(() => new Project(userId, projectName, desc, repositoryUrl, image, readme, platform));
        }

        [Fact]
        public void CreateProjectNoDescTest()
        {
            var userId = 233;
            var projectName = "测试";
            var desc = default(string);
            var repositoryUrl = "https://hahamango.cn";
            var image = "https://";
            var readme = "testReadme";
            var platform = "github";

            Assert.Throws<ArgumentNullException>(() => new Project(userId, projectName, desc, repositoryUrl, image, readme, platform));
        }

        [Fact]
        public void CreateProjectNoPlatformTest()
        {
            var userId = 233;
            var projectName = "测试";
            var desc = "测试描述";
            var repositoryUrl = "https://hahamango.cn";
            var image = "https://";
            var readme = "testReadme";
            var platform = default(string);

            Assert.Throws<ArgumentNullException>(() => new Project(userId, projectName, desc, repositoryUrl, image, readme, platform));
        }

        [Fact]
        public void DeleteProjectTest()
        {
            var userId = 233;
            var projectName = "测试";
            var desc = "测试描述";
            var repositoryUrl = "https://hahamango.cn";
            var image = "https://";
            var readme = "testReadme";
            var platform = "github";

            var project = new Project(userId, projectName, desc, repositoryUrl, image, readme, platform);

            project.Disable();

            Assert.Equal(ProjectStatusEnum.Close, project.Status);
        }
        
        [Fact]
        public void EnableProjectTest()
        {
            var userId = 233;
            var projectName = "测试";
            var desc = "测试描述";
            var repositoryUrl = "https://hahamango.cn";
            var image = "https://";
            var readme = "testReadme";
            var platform = "github";

            var project = new Project(userId, projectName, desc, repositoryUrl, image, readme, platform);

            project.Enable();

            Assert.Equal(ProjectStatusEnum.Open, project.Status);
        }

        [Fact]
        public void EditProjectTest()
        {
            var userId = 233;
            var projectName = "测试";
            var desc = "测试描述";
            var repositoryUrl = "https://hahamango.cn";
            var image = "https://";
            var readme = "testReadme";
            var platform = "github";

            var project = new Project(userId, projectName, desc, repositoryUrl, image, readme, platform);

            var newProjectName = "新测试";
            var newDesc = "新描述";
            var newRepositoryUrl = "http://new";
            var newImage = "https://newimage";
            var newReadme = "newreadme";
            var newPlatform = "gitlab";

            project.EditProjectInfo(newProjectName, newDesc, newRepositoryUrl, newImage, newReadme, newPlatform);

            Assert.Equal(newProjectName, project.ProjectInfo.ProjectName);
            Assert.Equal(newDesc, project.ProjectInfo.Desc);
            Assert.Equal(newRepositoryUrl, project.ProjectInfo.RepositoryUrl);
            Assert.Equal(newImage, project.ProjectInfo.Image);
            Assert.Equal(newReadme, project.ProjectInfo.README);
            Assert.Equal(newPlatform, project.ProjectInfo.Platform);
        }
    }
}
