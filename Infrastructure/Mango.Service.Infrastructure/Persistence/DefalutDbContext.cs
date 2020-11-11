using Mango.EntityFramework;
using Mango.EntityFramework.Abstractions;
using Mango.EntityFramework.BaseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Service.Infrastructure.Persistence
{
    public abstract class DefalutDbContext : BaseDbContext, IEfContextWork
    {
        public DefalutDbContext() { }

        public DefalutDbContext(DbContextOptions options) : base(options)
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
