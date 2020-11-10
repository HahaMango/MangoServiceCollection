using Mango.EntityFramework.Abstractions;
using Mango.EntityFramework.BaseEntity;
using Mango.Service.Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Infrastructure.Persistence
{
    /// <summary>
    /// 包含领域事件相关方法的聚合根
    /// </summary>
    public class AggregateRoot : SnowFlakeEntity, IAggregateRoot
    {
        private List<IDomainEvent> _domainEvents;
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

        /// <summary>
        /// 添加领域事件
        /// </summary>
        /// <param name="domainEvent"></param>
        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// 移除领域事件
        /// </summary>
        /// <param name="domainEvent"></param>
        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            if(_domainEvents == null)
            {
                return;
            }
            _domainEvents.Remove(domainEvent);
        }
    }
}
