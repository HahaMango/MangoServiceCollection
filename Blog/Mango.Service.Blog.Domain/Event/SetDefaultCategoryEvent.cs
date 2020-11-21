using Mango.Service.Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Domain.Event
{
    public class SetDefaultCategoryEvent : IDomainEvent
    {
        public long CategoryId { get; private set; }

        public SetDefaultCategoryEvent(long categoryId)
        {
            if (categoryId <= 0) throw new ArgumentException(nameof(categoryId));

            CategoryId = categoryId;
        }
    }
}
