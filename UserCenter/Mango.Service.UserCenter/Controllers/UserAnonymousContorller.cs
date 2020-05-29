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
using Mango.Core.ControllerAbstractions;
using Mango.Service.UserCenter.Abstraction.Models.Dto;
using Mango.Service.UserCenter.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.UserCenter.Controllers
{
    /// <summary>
    /// 用户无认证controller（登陆，注册用）
    /// </summary>
    public class UserAnonymousContorller : MangoUserApiController
    {
        private readonly IUserService _userService;

        public UserAnonymousContorller(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/usercenter/signup")]
        public async Task<ApiResult> SignUpWithUserNamePasswordAsync([FromBody]UserNamePasswordSignUpRequest request)
        {
            //core层加一个模型验证错误响应
            if (!ModelState.IsValid)
                return Error();
            return await _userService.SignUpWithUserNamePasswordAsync(request);
        }

        /// <summary>
        /// 用户登陆 返回jwt令牌
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/usercenter/login")]
        public async Task<ApiResult<string>> LoginWithUserNamePasswordAsync([FromBody]UserNamePasswordLoginRequest request)
        {
            if (!ModelState.IsValid)
                return Error<string>();
            return await _userService.LoginWithUserNamePasswordAsync(request);
        }
    }
}
