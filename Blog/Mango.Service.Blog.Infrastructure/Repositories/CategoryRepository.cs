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
using Mango.Service.Blog.Domain.AggregateModel.CategoryAggreate;
using Mango.Service.Blog.Domain.AggregateModel.Enum;
using Mango.Service.Blog.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Infrastructure.Repositories
{
    /// <summary>
    /// 文章分类仓储实现
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlogDbContext _context;

        public CategoryRepository(BlogDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task AddAsync(Category o)
        {
            await _context.Categories.AddAsync(o);
        }

        public async Task<Category> GetByIdAsync(long id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<IEnumerable<Category>> QueryUserCategoryAsync(long userId)
        {
            return await _context.Categories
                .Where(item => item.BloggerId == userId && item.Status == EntityStatusEnum.Available)
                .ToListAsync();
        }

        public async Task<Category> QueryUserDefaultCategoryAsync(long userId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(item => item.BloggerId == userId && item.IsDefault == 1);
        }

        public Task RemoveAsync(Category o)
        {
            o.Delete();
            _context.Categories.Update(o);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Category o)
        {
            _context.Categories.Update(o);
            return Task.CompletedTask;
        }
    }
}
