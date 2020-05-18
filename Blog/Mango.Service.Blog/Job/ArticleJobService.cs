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

using Mango.Service.Blog.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Job
{
    /// <summary>
    /// 后台作业
    /// </summary>
    public class ArticleJobService : BackgroundService
    {
        private readonly ILogger<ArticleJobService> _logger;
        private readonly IServiceProvider _service;

        private Timer _timer;

        public ArticleJobService(ILogger<ArticleJobService> logger,IServiceProvider service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// 实现定时从redis写回到mysql
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(TimeJob, null, TimeSpan.Zero, TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        /// <summary>
        /// 每时间间隔执行
        /// </summary>
        /// <param name="state"></param>
        private void TimeJob(object state)
        {
            _logger.LogInformation($"开始执行作业:{DateTime.Now}");

            try
            {
                using var scope = _service.CreateScope();
                var jobService = scope.ServiceProvider.GetService<IJobService>();
                jobService.WriteBackAsync().Wait();
            }
            catch(Exception ex)
            {
                _logger.LogError($"执行作业异常:{DateTime.Now},message={ex.Message}");
            }
        }
    }
}
