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
    /// 用户controller
    /// </summary>
    [Authorize]
    [Route("api/usercenter/")]
    public class UserController : MangoUserApiController
    {
        private readonly IUserService _userService;
        private readonly IUserPasswordService _userPasswordService;

        public UserController(
            IUserService userService,
            IUserPasswordService userPasswordService)
        {
            _userService = userService;
            _userPasswordService = userPasswordService;
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<ApiResult> EditUserInfoAsync([FromBody]EditUserInfoRequest request)
        {
            var user = GetUser();
            if (user == null)
                return AuthorizeError();

            return await _userService.EditUserInfoAsync(request, user.UserId);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("changepassword")]
        public async Task<ApiResult> ChangePasswordAsync([FromBody]ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return Error();
            var user = GetUser();
            if (user == null)
                return AuthorizeError();

            return await _userPasswordService.ChangePasswordAsync(request, user.UserId);
        }
    }
}
