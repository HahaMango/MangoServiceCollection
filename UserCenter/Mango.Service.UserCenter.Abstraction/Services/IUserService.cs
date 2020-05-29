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

using Mango.Core.ApiResponse;
using Mango.Service.UserCenter.Abstraction.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Service.UserCenter.Abstraction.Services
{
    /// <summary>
    /// 用户服务接口（注意：此处暂时采用绝对信任前端的方案，登出，修改密码等都将在前端主动丢弃jwt令牌。jwt维持其无状态的特性。）
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 用户名密码注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult> SignUpWithUserNamePasswordAsync(UserNamePasswordSignUpRequest request);

        /// <summary>
        /// 用户名密码登陆
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<string>> LoginWithUserNamePasswordAsync(UserNamePasswordLoginRequest request);

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult> EditUserInfoAsync(EditUserInfoRequest request, long userId);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult> ChangePasswordAsync(ChangePasswordRequest request, long userId);

        /// <summary>
        /// 通过用户Id查询用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult<UserInfoResponse>> QueryUserInfoById(long userId);
    }
}
