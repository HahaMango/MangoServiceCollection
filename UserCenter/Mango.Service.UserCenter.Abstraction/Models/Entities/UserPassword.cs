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

using Mango.Core.EntityFramework.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mango.Service.UserCenter.Abstraction.Models.Entities
{
    /// <summary>
    /// 用户登陆密码表
    /// </summary>
    [Table("user_password")]
    public class UserPassword : SnowFlakeEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 密码哈希散列
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// 密码强度
        /// </summary>
        public int PasswordStrength { get; set; }

        /// <summary>
        /// 使用状态 0：删除 1：正常（正在使用）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public UserPassword() { }

        /// <summary>
        /// 设置Id
        /// </summary>
        /// <param name="set"></param>
        public UserPassword(bool set)
        {
            base.SetId();
        }
    }
}
