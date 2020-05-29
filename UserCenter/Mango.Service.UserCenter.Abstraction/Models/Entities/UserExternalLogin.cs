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

using Mango.Core.EntityFramework.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mango.Service.UserCenter.Abstraction.Models.Entities
{
    /// <summary>
    /// 用户关联第三方登陆表
    /// </summary>
    [Table("user_external_login")]
    public class UserExternalLogin : SnowFlakeEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 登陆第三方提供方
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// 登陆第三方用户Key
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// 登陆第三方显示名称
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 状态 0：删除 1：正常
        /// </summary>
        public int Status { get; set; }

        public UserExternalLogin() { }

        /// <summary>
        /// 设置Id
        /// </summary>
        /// <param name="set"></param>
        public UserExternalLogin(bool set)
        {
            base.SetId();
        }
    }
}
