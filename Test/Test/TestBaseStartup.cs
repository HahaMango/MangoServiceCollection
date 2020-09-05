using Mango.Core.Extension;
using Mango.EntityFramework.Abstractions;
using Mango.Service.Blog.Abstractions.Models.Entities;
using Mango.Service.Blog.Abstractions.Repositories;
using Mango.Service.Blog.Repositories;
using Mango.Service.Blog.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Test
{
    /// <summary>
    /// ���Գ�ʼ����
    /// </summary>
    public abstract class TestBaseStartup
    {
        protected readonly ITestOutputHelper _output;

        public TestBaseStartup(ITestOutputHelper testOutputHelper)
        {
            _output = testOutputHelper;
            _output.WriteLine("��ʼ��");
        }

        public virtual IServiceProvider InitTestEnv()
        {
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(DateTime.Now.Ticks.ToString())
                .Options;
            var dbcontext = new BlogDbContext(options);

            var serviceCollection = new ServiceCollection();
            #region ���DI
            serviceCollection.AddAutoMapper();
            #endregion

            #region mock����
            InitMock(dbcontext, serviceCollection);
            #endregion

            return serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// ��ʼ��mock����
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="services"></param>
        public abstract void InitMock(BlogDbContext dbContext, IServiceCollection services);
    }
}
