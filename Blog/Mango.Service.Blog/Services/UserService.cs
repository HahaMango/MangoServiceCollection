/*--------------------------------------------------------------------------*/
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
using Mango.Core.Enums;
using Mango.Core.Extension;
using Mango.EntityFramework.Abstractions;
using Mango.Service.Blog.Abstractions.Models.Dto;
using Mango.Service.Blog.Abstractions.Models.Entities;
using Mango.Service.Blog.Abstractions.Repositories;
using Mango.Service.Blog.Abstractions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Services
{
    /// <summary>
    /// 用户服务实现类
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        private readonly IUserRepository _userRepository;
        private readonly IEfContextWork _work;

        public UserService(
            ILogger<UserService> logger,
            IUserRepository userRepository,
            IEfContextWork work)
        {
            _logger = logger;
            _userRepository = userRepository;
            _work = work;
        }

        /// <summary>
        /// 通过Id查询用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult<UserInfoResponse>> QueryUserByIdAsync(long userId)
        {
            var response = new ApiResult<UserInfoResponse>();
            try
            {
                var user = await _userRepository.TableNotTracking
                    .FirstOrDefaultAsync(item => item.Id == userId && item.Status == 1);
                if(user == null)
                {
                    response.Code = Code.Error;
                    response.Message = "用户不存在或被删除";
                    return response;
                }

                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = user.MapTo<UserInfoResponse>();
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"查询用户信息异常;method={nameof(QueryUserByIdAsync)};param={userId};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询用户信息异常：{ex.Message}";
                return response;
            }
        }
    }
}
