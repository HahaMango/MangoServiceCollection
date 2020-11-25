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
using Mango.Service.Blog.Domain.AggregateModel.ArticleAggregate;
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
    /// 文章仓储实现类
    /// </summary>
    public class ArticleRepository : IArticleRepository
    {
        private readonly BlogDbContext _context;

        public ArticleRepository(BlogDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task AddAsync(Article o)
        {
            await _context.Articles.AddAsync(o);
        }

        public async Task<Article> GetByIdAsync(long id)
        {
            return await _context.Articles.Where(item => item.Id == id)
                .Include(a => a.Categories)
                .FirstOrDefaultAsync();
        }

        public Task RemoveAsync(Article o)
        {
            o.Delete();
            _context.Articles.Update(o);
            _context.CategoryAssociations.RemoveRange(o.Categories);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Article o)
        {
            _context.Articles.Update(o);
            _context.CategoryAssociations.UpdateRange(o.Categories);
            return Task.CompletedTask;
        }
    }
}
