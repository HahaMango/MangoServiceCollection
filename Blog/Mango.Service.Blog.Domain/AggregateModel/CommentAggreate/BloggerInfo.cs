﻿using Mango.Service.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Domain.AggregateModel.CommentAggreate
{
    /// <summary>
    /// 评论用户信息值对象
    /// </summary>
    public class BloggerInfo : IValueObject
    {
        public long? UserId { get; private set; }

        public string UserName { get; private set; }

        public BloggerInfo() { }

        public BloggerInfo(long? userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }
    }
}
