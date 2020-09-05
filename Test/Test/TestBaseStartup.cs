/*--------------------------------------------------------------------------
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
    /// 测试初始化类
    /// </summary>
    public abstract class TestBaseStartup
    {
        protected readonly ITestOutputHelper _output;

        public TestBaseStartup(ITestOutputHelper testOutputHelper)
        {
            _output = testOutputHelper;
            _output.WriteLine("初始化");
        }

        public virtual IServiceProvider InitTestEnv()
        {
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(DateTime.Now.Ticks.ToString())
                .Options;
            var dbcontext = new BlogDbContext(options);

            var serviceCollection = new ServiceCollection();
            #region 添加DI
            serviceCollection.AddAutoMapper();
            #endregion

            #region mock对象
            InitMock(dbcontext, serviceCollection);
            #endregion

            return serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// 初始化mock对象
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="services"></param>
        public abstract void InitMock(BlogDbContext dbContext, IServiceCollection services);

        /// <summary>
        /// 获取mock的logger
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMock<ILogger<T>> GetMockLogger<T>()
        {
            var log = new Mock<ILogger<T>>();
            log.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                    .Callback(new InvocationAction(invocation =>
                    {
                        var logLevel = (LogLevel)invocation.Arguments[0];
                        var eventId = (EventId)invocation.Arguments[1];
                        var state = invocation.Arguments[2];
                        var exception = (Exception?)invocation.Arguments[3];
                        var formatter = invocation.Arguments[4];

                        var invokeMethod = formatter.GetType().GetMethod("Invoke");
                        var logMessage = (string?)invokeMethod?.Invoke(formatter, new[] { state, exception });

                        _output.WriteLine(logMessage);
                    }));

            return log;
        }
    }
}
