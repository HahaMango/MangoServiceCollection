using Mango.EntityFramework.Abstractions;
using Mango.Service.Blog.Abstractions.Repositories;
using Mango.Service.Blog.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var moqLogger = new Mock<ILogger<UserService>>();
            var moqUr = new Mock<IUserRepository>();
            var moqef = new Mock<IEfContextWork>();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(moqLogger.Object);
            serviceCollection.AddSingleton(moqUr.Object);
            serviceCollection.AddSingleton(moqef.Object);

            var provider = serviceCollection.BuildServiceProvider();

            var logger = provider.GetRequiredService<ILogger<UserService>>();
            var userRepository = provider.GetRequiredService<IUserRepository>();
            var efwork = provider.GetRequiredService<IEfContextWork>();

            var userService = new UserService(logger, userRepository, efwork);
            var result = await userService.QueryUserByIdAsync(1000);

            var i = 0;
        }
    }
}
