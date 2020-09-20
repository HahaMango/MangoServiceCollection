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
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mango.Service.UserCenter.Abstraction.Models.Dto
{
    class UserRequestDto
    {
    }

    #region 普通用户名密码注册请求
    public class UserNamePasswordSignUpRequest
    {
        /// <summary>
        /// 唯一
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 密码明文
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 密码强度
        /// </summary>
        [Required]
        public int PasswordStrength { get; set; }
    }
    #endregion

    #region 普通用户名密码登陆请求
    public class UserNamePasswordLoginRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 密码明文
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 请求令牌的请求域
        /// </summary>
        public string Audience { get; set; }
    }
    #endregion

    #region 修改用户信息
    public class EditUserInfoRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
    #endregion

    #region 修改用户密码
    public class ChangePasswordRequest
    {
        /// <summary>
        /// 新的密码文明
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 新的密码强度
        /// </summary>
        public int PasswordStrength { get; set; }

        /// <summary>
        /// 旧密码明文
        /// </summary>
        [Required]
        public string OldPassword { get; set; }

        /// <summary>
        /// 密码确认密码
        /// </summary>
        [Required]
        public string PasswordConfirm { get; set; }
    }
    #endregion

    #region 查询用户关于页markdown请求
    public class QueryUserAboutRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
    }
    #endregion

    #region 更新用户关于页markdown请求
    public class UpdateUserAboutRequest
    {
        /// <summary>
        /// 关于页描述（格式：markdown）
        /// </summary>
        public string Desc { get; set; }
    }
    #endregion
}
