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

using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.UserCenter.Abstraction.Models.Dto
{
    class UserResponseDto
    {
    }

    #region 用户信息相应类
    public class UserInfoResponse
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
        /// 手机号（每个手机号绑定一个用户）
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 头像Url
        /// </summary>
        public string HeadUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 密码强度
        /// </summary>
        public int PasswordStrength { get; set; }

        /// <summary>
        /// 用户绑定的第三方登陆提供商
        /// </summary>
        public IEnumerable<ExternalLoginInfo> ExternalLoginInfos { get; set; }

        /// <summary>
        /// 最近登陆时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
    }

    public class ExternalLoginInfo
    {
        /// <summary>
        /// 第三方登陆提供器显示名称
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// 第三方登陆提供商图片
        /// </summary>
        public string ProviderImageUrl { get; set; }
    }
    #endregion
}
