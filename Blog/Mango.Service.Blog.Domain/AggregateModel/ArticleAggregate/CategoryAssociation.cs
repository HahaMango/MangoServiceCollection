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

using Mango.EntityFramework.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Domain.AggregateModel.ArticleAggregate
{
    /// <summary>
    /// 文章分类关联实体
    /// </summary>
    public class CategoryAssociation : SnowFlakeEntity
    {
        public long ArticleId { get; private set; }

        public long CategoryId { get; private set; }

        protected CategoryAssociation() { }

        /// <summary>
        /// 创建文章分类关联
        /// </summary>
        /// <param name="categoryId"></param>
        public CategoryAssociation(long articleId, long categoryId)
        {
            ArticleId = articleId;
            CategoryId = categoryId;
        }
    }
}
