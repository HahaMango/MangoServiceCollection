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

using Mango.Service.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Domain.AggregateModel.ArticleAggregate
{
    /// <summary>
    /// 文章信息值对象
    /// </summary>
    public class ArticleInfo : IValueObject
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; }

        public ArticleInfo() { }

        public ArticleInfo(string title,string desc)
        {
            Title = title;
            Describe = desc;
        }
    }
}
