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
using Mango.Service.Blog.Abstractions.Models.Dto;
using Mango.Service.Blog.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Controllers
{
    /// <summary>
    /// 用户信息controller
    /// </summary>
    public class UserController : MangoUserApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 通过用户Id 查询用户基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/user/info")]
        public async Task<ApiResult<UserInfoResponse>> QueryUserByIdAsync()
        {
            var user = GetUser();
            if(user == null)
            {
                return AuthorizeError<UserInfoResponse>();
            }
            return await _userService.QueryUserByIdAsync(user.UserId);
        }
    }
}
