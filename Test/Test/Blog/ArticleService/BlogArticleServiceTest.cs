using Mango.Core.Enums;
using Mango.EntityFramework;
using Mango.EntityFramework.Abstractions;
using Mango.Service.Blog.Abstractions.Models.Dto;
using Mango.Service.Blog.Abstractions.Models.Entities;
using Mango.Service.Blog.Abstractions.Repositories;
using Mango.Service.Blog.Abstractions.Services;
using Mango.Service.Blog.Repositories;
using Mango.Service.Blog.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Test.Blog.ArticleService
{
    /// <summary>
    /// 博客文章单元测试
    /// </summary>
    public class BlogArticleServiceTest : TestBaseStartup
    {
        private readonly IServiceProvider _serviceProvider;

        public BlogArticleServiceTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _serviceProvider = InitTestEnv();
        }

        #region 初始化

        public override void InitMock(BlogDbContext dbContext, IServiceCollection services)
        {
            var log = new Mock<ILogger<Mango.Service.Blog.Services.ArticleService>>();
            log.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                    .Callback(new InvocationAction(invocation =>
                    {
                        var logLevel = (LogLevel)invocation.Arguments[0]; // The first two will always be whatever is specified in the setup above
                        var eventId = (EventId)invocation.Arguments[1];  // so I'm not sure you would ever want to actually use them
                        var state = invocation.Arguments[2];
                        var exception = (Exception?)invocation.Arguments[3];
                        var formatter = invocation.Arguments[4];

                        var invokeMethod = formatter.GetType().GetMethod("Invoke");
                        var logMessage = (string?)invokeMethod?.Invoke(formatter, new[] { state, exception });

                        _output.WriteLine(logMessage);
                    }));
            var articleCache = new Mock<IArticleCacheService>();
            var work = new EfContextWork<BlogDbContext>(dbContext);
            var articleRepository = new ArticleRepository(dbContext);
            var articleDetailRepository = new ArticleDetailRepository(dbContext);
            var categoryRepository = new CategoryRepository(dbContext);
            var userRepository = new UserRepository(dbContext);

            var user = new User()
            {
                Id = 1264467991328854016,
                UserName = "Chiva",
                CreateTime = DateTime.Now,
                Status = 1
            };
            userRepository.Insert(user);
            work.SaveChanges();

            var articleService = new Mango.Service.Blog.Services.ArticleService(log.Object, articleRepository, articleDetailRepository, work, userRepository, categoryRepository,articleCache.Object);

            services.AddSingleton<IEfContextWork>(work);
            services.AddSingleton<IArticleRepository>(articleRepository);
            services.AddSingleton<IArticleDetailRepository>(articleDetailRepository);
            services.AddSingleton<ICategoryRepository>(categoryRepository);
            services.AddSingleton<IUserRepository>(userRepository);
            services.AddSingleton<IArticleService>(articleService);
        }

        #endregion

        /// <summary>
        /// 测试添加文章
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(BlogTestData.AddArticleData),MemberType = typeof(BlogTestData))]
        public async Task AddArticleTest(AddArticleRequest request,string userId,int code)
        {
            #region 数据准备
            var articleServicee = _serviceProvider.GetRequiredService<IArticleService>();
            #endregion

            #region 执行
            var result = await articleServicee.AddArticleAsync(request, Convert.ToInt64(userId));
            #endregion

            #region 断言
            Assert.NotNull(result);
            Assert.Equal(code, (int)result.Code);
            #endregion
        }
    }
}
