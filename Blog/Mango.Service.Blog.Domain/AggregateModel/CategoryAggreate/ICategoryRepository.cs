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

using Mango.EntityFramework.Abstractions;
using Mango.EntityFramework.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Domain.AggregateModel.CategoryAggreate
{
    /// <summary>
    /// 文章分类仓储接口
    /// </summary>
    public interface ICategoryRepository : IAggregateRepository<Category, long>
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// 查询用户的默认文章分类
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Category> QueryUserDefaultCategoryAsync(long userId);

        /// <summary>
        /// 查询某用户的分类列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Category>> QueryUserCategoryAsync(long userId);

        /// <summary>
        /// 批量查询
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<Category>> GetByIdsAsync(params long[] ids);
    }
}
