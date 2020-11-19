using Mango.EntityFramework;
using Mango.EntityFramework.Abstractions;
using Mango.EntityFramework.BaseEntity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mango.Service.Infrastructure.Persistence
{
    public abstract class DefalutDbContext : BaseDbContext, IEfContextWork
    {
        private readonly IMediator _mediator;

        private IDbContextTransaction _dbContextTransaction;

        public DefalutDbContext() { }

        public DefalutDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public virtual IDbContextTransaction BeginTransaction()
        {
            if (_dbContextTransaction != null)
            {
                return _dbContextTransaction;
            }
            _dbContextTransaction = Database.BeginTransaction();
            return _dbContextTransaction;
        }

        public virtual async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_dbContextTransaction != null)
            {
                return _dbContextTransaction;
            }
            _dbContextTransaction = await Database.BeginTransactionAsync();
            return _dbContextTransaction;
        }

        public virtual void Commit()
        {
            if (_dbContextTransaction == null) throw new ArgumentNullException(nameof(_dbContextTransaction));

            try
            {
                base.SaveChanges();
                _dbContextTransaction.Commit();
            }
            catch
            {
                this.Rollback();
                throw;
            }
            finally
            {
                if(_dbContextTransaction != null)
                {
                    _dbContextTransaction.Dispose();
                    _dbContextTransaction = null;
                }
            }
        }

        public virtual async Task CommitAsync()
        {
            if (_dbContextTransaction == null) throw new ArgumentNullException(nameof(_dbContextTransaction));

            try
            {
                await base.SaveChangesAsync();
                await _dbContextTransaction.CommitAsync();
            }
            catch
            {
                await this.RollbackAsync();
                throw;
            }
            finally
            {
                if (_dbContextTransaction != null)
                {
                    await _dbContextTransaction.DisposeAsync();
                    _dbContextTransaction = null;
                }
            }
        }

        public virtual void Rollback()
        {
            try
            {
                _dbContextTransaction?.Rollback();
            }
            catch
            {
                if (_dbContextTransaction != null)
                {
                    _dbContextTransaction.Dispose();
                    _dbContextTransaction = null;
                }
            }
        }

        public virtual async Task RollbackAsync()
        {
            try
            {
                await _dbContextTransaction?.RollbackAsync();
            }
            catch
            {
                if (_dbContextTransaction != null)
                {
                    await _dbContextTransaction.DisposeAsync();
                    _dbContextTransaction = null;
                }
            }
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            var es = base.ChangeTracker.Entries<AggregateRoot>()
                .Where(item => item.Entity != null && item.Entity.DomainEvents != null && item.Entity.DomainEvents.Any())
                .Select(item => item.Entity.DomainEvents);
            foreach(var e in es)
            {
                foreach(var domainEvent in e)
                {
                    await _mediator.Publish(domainEvent);
                }
            }
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
