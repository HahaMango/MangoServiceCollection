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

using Mango.Core.ApiResponse;
using Mango.Service.UserCenter.Abstraction.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Service.UserCenter.Abstraction.Services
{
    /// <summary>
    /// 用户密码服务接口
    /// </summary>
    public interface IUserPasswordService
    {
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult> ChangePasswordAsync(ChangePasswordRequest request, long userId);

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> VerifyPasswordAsync(long userId, string password);

        /// <summary>
        /// 生成密码哈希散列
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string PasswordToHash(string password);
    }
}
