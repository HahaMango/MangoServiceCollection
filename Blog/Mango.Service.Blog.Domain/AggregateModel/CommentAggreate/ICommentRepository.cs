﻿/*--------------------------------------------------------------------------
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

using Mango.EntityFramework.Abstractions;
using Mango.EntityFramework.Abstractions.Repositories;

namespace Mango.Service.Blog.Domain.AggregateModel.CommentAggreate
{
    /// <summary>
    /// 文章评论仓储接口
    /// </summary>
    public interface ICommentRepository : IAggregateRepository<Comment, long>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
