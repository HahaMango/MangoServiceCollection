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

using Mango.EntityFramework;
using Mango.EntityFramework.Abstractions;
using Mango.EntityFramework.BaseEntity;
using Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Service.OpenSource.Infrastructure.DbContext
{
    public class OpenSourceDbContext: BaseDbContext, IEfContextWork
    {
        public DbSet<Project> Projects { get; set; }

        public OpenSourceDbContext()
        {

        }

        public OpenSourceDbContext(DbContextOptions options) : base(options)
        {

        }

        public IDbContextTransaction BeginTransaction()
        {
            return base.Database.BeginTransaction();
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return base.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            base.Database.CommitTransaction();
        }

        public void Rollback()
        {
            base.Database.RollbackTransaction();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assemblies = GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
                    .Where(type => type.IsClass)
                    .Where(type => type.BaseType != null)
                    .Where(type => typeof(Entity).IsAssignableFrom(type));//&& !typeof(IDbTable).IsSubclassOf(type))直接或间接的实现

                foreach (var type in types)
                {
                    if (modelBuilder.Model.FindEntityType(type) != null || type.Name == "Entity" || type.Name == "SnowFlakeEntity" || type.Name == "AggregateRoot")
                        continue;
                    modelBuilder.Model.AddEntityType(type);
                }
            }
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
        }

        /// <summary>
        /// 加载Mango开头的引用程序集
        /// </summary>
        /// <returns></returns>
        private List<Assembly> GetAssemblies()
        {
            var result = new List<Assembly>();
            var assemblies = DependencyContext.Default.CompileLibraries
                .Where(item => item.Name.StartsWith("Mango"))
                .ToList();
            foreach (var assembly in assemblies)
            {
                result.Add(Assembly.Load(assembly.Name));
            }

            return result;
        }
    }
}
