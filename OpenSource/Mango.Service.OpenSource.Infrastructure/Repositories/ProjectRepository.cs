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

using FreeRedis;
using Mango.EntityFramework.Abstractions;
using Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate;
using Mango.Service.OpenSource.Infrastructure.Config;
using Mango.Service.OpenSource.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Service.OpenSource.Infrastructure.Repositories
{
    /// <summary>
    /// 开源项目聚合根仓储实现类
    /// </summary>
    public class ProjectRepository : IProjectRepository
    {
        private readonly OpenSourceDbContext _dbContext;
        private readonly RedisClient _redisClient;

        public IEfContextWork UnitOfWork
        {
            get
            {
                return _dbContext;
            }
        }

        public ProjectRepository(OpenSourceDbContext openSourceDbContext,RedisClient redisClient)
        {
            _dbContext = openSourceDbContext;
            _redisClient = redisClient;
        }

        public async Task AddAsync(Project o)
        {
            await _dbContext.Projects.AddAsync(o);
        }

        public async Task<Project> GetByIdAsync(long id)
        {
            return await _dbContext.Projects.Where(item => item.Id == id).FirstOrDefaultAsync();
        }

        public Task RemoveAsync(Project o)
        {
            _dbContext.Projects.Remove(o);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Project o)
        {
            _dbContext.Projects.Update(o);
            return Task.CompletedTask;
        }
    }
}
