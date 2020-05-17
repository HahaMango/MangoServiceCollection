/*--------------------------------------------------------------------------*/
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
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mango.Service.Blog.Abstractions.Models.Entities
{
    /// <summary>
    /// 文章分类表
    /// </summary>
    [Table("category")]
    public class Category : SnowFlakeEntity
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 状态 0：删除 1：正常
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否默认分类
        /// </summary>
        public int IsDefault { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Category() { }

        /// <summary>
        /// 设置Id
        /// </summary>
        /// <param name="set"></param>
        public Category(bool set)
        {
            base.SetId();
        }
    }
}
