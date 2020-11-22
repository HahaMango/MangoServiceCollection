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
using Mango.Service.Blog.Domain.AggregateModel.Enum;
using Mango.Service.Blog.Domain.Event;
using Mango.Service.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Domain.AggregateModel.CategoryAggreate
{
    /// <summary>
    /// 文章分类聚合
    /// </summary>
    public class Category: AggregateRoot
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        private long _userId;

        private DateTime _createTime;

        private DateTime? _updateTime;

        public EntityStatusEnum Status { get; private set; }

        /// <summary>
        /// 是否默认分类 0：否 1：是
        /// </summary>
        public int IsDefault { get; private set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; private set; }


        protected Category()
        {

        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="categoryName"></param>
        public Category(long userId, string categoryName)
        {
            Verification(categoryName);

            SetId();
            _userId = userId;
            CategoryName = categoryName;
            _createTime = DateTime.Now;
            Status = EntityStatusEnum.Available;
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        public void Delete()
        {
            Status = EntityStatusEnum.Deleted;
        }

        /// <summary>
        /// 编辑分类
        /// </summary>
        /// <param name="categoryName"></param>
        public void Edit(string categoryName)
        {
            Verification(categoryName);

            CategoryName = categoryName;
        }

        /// <summary>
        /// 设置默认分类
        /// </summary>
        public void SetDefault()
        {
            IsDefault = 1;

            var domainEvent = new SetDefaultCategoryEvent(Id);
            AddDomainEvent(domainEvent);
        }

        /// <summary>
        /// 取消默认分类
        /// </summary>
        public void CancelDefault()
        {
            IsDefault = 0;
        }

        private void Verification(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName)) throw new ArgumentNullException(nameof(categoryName));
        }
    }
}
