/*--------------------------------------------------------------------------*/
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

using Mango.Core.Test;
using Mango.EntityFramework;
using Mango.EntityFramework.Abstractions;
using Mango.Service.Blog.Abstractions.Models.Dto;
using Mango.Service.Blog.Abstractions.Models.Entities;
using Mango.Service.Blog.Abstractions.Repositories;
using Mango.Service.Blog.Abstractions.Services;
using Mango.Service.Blog.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Mango.Service.Blog.Test.ArticleService
{
    public class BlogArticleServiceTest : BaseTestStartup<BlogDbContext>
    {
        #region 私有变量
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region 构造函数
        public BlogArticleServiceTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _serviceProvider = InitTestEnv();
        }
        #endregion

        #region 初始化mock配置
        public override void InitMock(BlogDbContext dbContext, IServiceCollection services)
        {
            var log = GetMockLogger<Mango.Service.Blog.Services.ArticleService>();
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

            var articleService = new Services.ArticleService(log.Object, articleRepository, articleDetailRepository, work, userRepository, categoryRepository, articleCache.Object);

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
        [MemberData(nameof(BlogTestData.AddArticleData), MemberType = typeof(BlogTestData))]
        public async Task AddArticleTest(AddArticleRequest request, string userId, int code)
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
