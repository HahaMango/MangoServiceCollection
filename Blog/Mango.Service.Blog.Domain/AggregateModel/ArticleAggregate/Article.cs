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
using Mango.Service.Blog.Domain.AggregateModel.CategoryAggreate;
using Mango.Service.Blog.Domain.AggregateModel.Enum;
using Mango.Service.Blog.Domain.Event;
using Mango.Service.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mango.Service.Blog.Domain.AggregateModel.ArticleAggregate
{
    /// <summary>
    /// 文章聚合
    /// </summary>
    public class Article: AggregateRoot
    {
        private long _bloggerId;

        private DateTime _createTime;

        private DateTime? _updateTime;

        private int _view;

        private int _comment;

        private int _like;

        private List<CategoryAssociation> _categories;

        /// <summary>
        /// 文章信息
        /// </summary>
        public ArticleInfo ArticleInfo { get; private set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public int IsTop { get; private set; }

        /// <summary>
        /// 状态
        /// </summary>
        public EntityStatusEnum Status { get; private set; }

        /// <summary>
        /// 文章分类
        /// </summary>
        public IReadOnlyList<CategoryAssociation> Categories => _categories;

        protected Article() { }

        /// <summary>
        /// 创建文章
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="content"></param>
        public Article(long userId, string title, string desc, string content, List<Category> categories)
        {
            Verification(title, desc, content);
            VerifyCategories(categories);
            SetId();

            _bloggerId = userId;
            Content = content;
            ArticleInfo = new ArticleInfo(title, desc);
            Status = EntityStatusEnum.Available;
            EditCategory(categories);
            _createTime = DateTime.Now;

            AddDomainEvent(new CreateArticleEvent(_bloggerId, Id));
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            Status = EntityStatusEnum.Deleted;
        }

        /// <summary>
        /// 置顶
        /// </summary>
        public void SetTop()
        {
            IsTop = 1;
        }

        /// <summary>
        /// 取消置顶
        /// </summary>
        public void CancelTop()
        {
            IsTop = 0;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="content"></param>
        /// <param name="categories"></param>
        public void Edit(string title, string desc, string content, List<Category> categories)
        {
            Verification(title, desc, content);
            VerifyCategories(categories);

            Content = content;
            ArticleInfo = new ArticleInfo(title, desc);
            EditCategory(categories);
        }

        /// <summary>
        /// 编辑文章分类
        /// </summary>
        /// <param name="categories"></param>
        public void EditCategory(List<Category> categories)
        {
            VerifyCategories(categories);

            if(_categories == null)
            {
                _categories = new List<CategoryAssociation>();
            }
            _categories.Clear();
            foreach(var c in categories)
            {
                var ca = new CategoryAssociation(Id, c.Id);
                _categories.Add(ca);
            }
        }

        private void Verification(string title, string desc, string content)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentNullException(nameof(title));
            if (string.IsNullOrEmpty(desc)) throw new ArgumentNullException(nameof(desc));
            if (string.IsNullOrEmpty(content)) throw new ArgumentNullException(nameof(content));
        }

        private void VerifyCategories(List<Category> categories)
        {
            if (categories == null || categories.Count <= 0)
            {
                throw new ArgumentException(nameof(categories));
            }
        }
    }
}
