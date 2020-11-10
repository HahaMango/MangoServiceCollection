using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Infrastructure.Event
{
    /// <summary>
    /// 领域事件标记接口
    /// </summary>
    public interface IDomainEvent : INotification
    {
    }
}
