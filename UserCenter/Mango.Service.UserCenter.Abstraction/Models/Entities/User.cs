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
    /// 用户表
    /// </summary>
    [Table("user")]
    public class User : SnowFlakeEntity
    {
        /// <summary>
        /// 用户名 (唯一的登陆用户名)
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 邮箱（每个邮箱绑定一个用户）
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 邮箱是否认证 0：未认证 1：认证
        /// </summary>
        public int EmailConfirm { get; set; }

        /// <summary>
        /// 手机号（每个手机号绑定一个用户）
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 手机是否认证 0：未认证 1：认证
        /// </summary>
        public int PhoneConfirm { get; set; }

        /// <summary>
        /// 头像Url
        /// </summary>
        public string HeadUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态 0：删除 1：正常
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后登陆IP
        /// </summary>
        public string LastloginIP { get; set; }

        public User() { }

        /// <summary>
        /// 设置Id
        /// </summary>
        /// <param name="set"></param>
        public User(bool set)
        {
            base.SetId();
        }
    }
}
