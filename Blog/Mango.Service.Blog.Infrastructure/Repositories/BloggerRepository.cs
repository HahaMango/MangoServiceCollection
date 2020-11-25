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
using Mango.Service.Blog.Domain.AggregateModel.BloggerAggreate;
using Mango.Service.Blog.Infrastructure.DbContext;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Infrastructure.Repositories
{
    public class BloggerRepository : IBloggerRepository
    {
        private readonly BlogDbContext _context;

        public BloggerRepository(BlogDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task AddAsync(Blogger o)
        {
            await _context.Bloggers.AddAsync(o);
        }

        public async Task<Blogger> GetByIdAsync(long id)
        {
            return await _context.Bloggers.FindAsync(id);
        }

        public Task RemoveAsync(Blogger o)
        {
            _context.Bloggers.Remove(o);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Blogger o)
        {
            _context.Bloggers.Update(o);
            return Task.CompletedTask;
        }
    }
}
