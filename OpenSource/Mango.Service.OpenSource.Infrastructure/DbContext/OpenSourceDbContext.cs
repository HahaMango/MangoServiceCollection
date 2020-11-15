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
using Mango.EntityFramework.BaseEntity;
using Mango.Service.Infrastructure.Persistence;
using Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate;
using Mango.Service.OpenSource.Infrastructure.Config;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.OpenSource.Infrastructure.DbContext
{
    public class OpenSourceDbContext: DefalutDbContext
    {
        private readonly RedisClient _redisClient;

        public DbSet<Project> Projects { get; set; }

        public OpenSourceDbContext()
        {

        }

        public OpenSourceDbContext(DbContextOptions options, RedisClient redisClient, IMediator mediator) : base(options, mediator)
        {
            _redisClient = redisClient;
        }

        public async override Task<int> SaveChangesAsync()
        {
            #region 清除缓存
            await _redisClient.DelAsync(CacheKeyConfig.ProjectList);
            var es = base.ChangeTracker.Entries<Entity>().Where(item => item.Entity != null);
            foreach(var e in es)
            {
                await _redisClient.DelAsync($"{CacheKeyConfig.ProjectDetail}:{e.Entity.Id}");
            }
            #endregion
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var e = modelBuilder.Entity<Project>();
            e.ToTable("project");
            var pip = e.OwnsOne(p => p.ProjectInfo);
            pip.Property(pi => pi.ProjectName).HasColumnName("ProjectName");
            pip.Property(pi => pi.Desc).HasColumnName("Desc");
            pip.Property(pi => pi.RepositoryUrl).HasColumnName("RepositoryUrl");
            pip.Property(pi => pi.Image).HasColumnName("Image");
            pip.Property(pi => pi.README).HasColumnName("Readme");
            pip.Property(pi => pi.Platform).HasColumnName("Platform");

            e.Property(p => p.Status).HasColumnName("Status");
            e.Property<long>("_userId").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("UserId");
            e.Property<DateTime>("_createTime").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("CreateTime");
            e.Property<DateTime?>("_updateTime").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("UpdateTime");
            e.Property<int>("_sortId").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("SortId");
        }
    }
}
