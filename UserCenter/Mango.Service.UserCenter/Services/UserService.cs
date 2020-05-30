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
using Mango.Core.Authentication.Jwt;
using Mango.Core.Encryption;
using Mango.Core.Enums;
using Mango.Core.Extension;
using Mango.Core.Serialization.Extension;
using Mango.EntityFramework.Abstractions;
using Mango.Infrastructure.Helper;
using Mango.Service.UserCenter.Abstraction.Models.Dto;
using Mango.Service.UserCenter.Abstraction.Models.Entities;
using Mango.Service.UserCenter.Abstraction.Repositories;
using Mango.Service.UserCenter.Abstraction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mango.Service.UserCenter.Services
{
    /// <summary>
    /// 用户服务接口实现
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        private readonly IUserRepository _userRepository;
        private readonly IUserPasswordRepository _userPasswordRepository;
        private readonly IUserExternalLoginRepository _userExternalLoginRepository;
        private readonly IEfContextWork _work;

        private readonly MangoJwtTokenHandler _mangoJwtTokenHandler;

        private readonly IUserPasswordService _userPasswordService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            ILogger<UserService> logger,
            IUserRepository userRepository,
            IUserPasswordRepository userPasswordRepository,
            MangoJwtTokenHandler mangoJwtTokenHandler,
            IUserPasswordService userPasswordService,
            IHttpContextAccessor httpContextAccessor,
            IEfContextWork work,
            IUserExternalLoginRepository userExternalLoginRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _userPasswordRepository = userPasswordRepository;
            _mangoJwtTokenHandler = mangoJwtTokenHandler;
            _userPasswordService = userPasswordService;
            _httpContextAccessor = httpContextAccessor;
            _work = work;
            _userExternalLoginRepository = userExternalLoginRepository;
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult> EditUserInfoAsync(EditUserInfoRequest request, long userId)
        {
            var response = new ApiResult();
            try
            {
                var user = await _userRepository.Table
                    .FirstOrDefaultAsync(item => item.Id == userId && item.Status == 1);
                if(user == null)
                {
                    response.Code = Code.Error;
                    response.Message = "找不到用户";
                    return response;
                }
                //设置用户名
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    var isExist = await _userRepository.TableNotTracking
                        .AnyAsync(item => item.UserName == request.UserName);
                    if (!isExist)
                    {
                        user.UserName = request.UserName;
                        user.UpdateTime = DateTime.Now;
                    }
                }
                //设置昵称
                if (!string.IsNullOrEmpty(request.NickName))
                {
                    user.NickName = request.NickName;
                    user.UpdateTime = DateTime.Now;
                }
                //设置邮箱
                if (!string.IsNullOrEmpty(request.Email))
                {
                    user.Email = request.Email;
                    user.EmailConfirm = 0;
                    user.UpdateTime = DateTime.Now;
                }
                //设置手机
                if (!string.IsNullOrEmpty(request.Phone))
                {
                    user.Phone = request.Phone;
                    user.PhoneConfirm = 0;
                    user.UpdateTime = DateTime.Now;
                }
                //头像
                if (!string.IsNullOrEmpty(request.HeadUrl))
                {
                    user.HeadUrl = request.HeadUrl;
                    user.UpdateTime = DateTime.Now;
                }
                //备注
                if (!string.IsNullOrEmpty(request.Remark))
                {
                    user.Remark = request.Remark;
                    user.UpdateTime = DateTime.Now;
                }
                await _work.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "编辑成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"编辑用户信息异常;method={nameof(EditUserInfoAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"编辑用户信息异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 用户名密码登陆 (jwt)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> LoginWithUserNamePasswordAsync(UserNamePasswordLoginRequest request)
        {
            var response = new ApiResult<string>();
            try
            {
                var user = await _userRepository.Table
                    .FirstOrDefaultAsync(item => item.UserName == request.UserName && item.Status == 1);
                if(user == null)
                {
                    response.Code = Code.Error;
                    response.Message = "该用户名不存在";
                    return response;
                }
                if(!await _userPasswordService.VerifyPasswordAsync(user.Id, request.Password))
                {
                    response.Code = Code.Error;
                    response.Message = "密码错误";
                    return response;
                }
                //获取令牌
                var token = _mangoJwtTokenHandler.IssuedToken<User,long>(user, new Claim("aud", request.Audience));

                user.LastLoginTime = DateTime.Now;
                user.LastloginIP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                await _work.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "登陆成功";
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"更改密码异常;method={nameof(LoginWithUserNamePasswordAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"更改密码异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 通过用户Id查找用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult<UserInfoResponse>> QueryUserInfoById(long userId)
        {
            var response = new ApiResult<UserInfoResponse>();
            try
            {
                var user = await (from u in _userRepository.TableNotTracking
                                  join up in _userPasswordRepository.TableNotTracking on u.Id equals up.UserId
                                  where u.Id == userId
                                  select new
                                  {
                                      u,
                                      up.PasswordStrength
                                  })
                                  .FirstOrDefaultAsync();
                var result = user.u.MapTo<UserInfoResponse>();
                var externalLogins = await _userExternalLoginRepository.TableNotTracking
                    .Where(item => item.UserId == userId && item.Status == 1)
                    .ToListAsync();
                result.ExternalLoginInfos = externalLogins.MapToList<ExternalLoginInfo>();

                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = result;
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"查询用户信息异常;method={nameof(LoginWithUserNamePasswordAsync)};param={userId};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询用户信息异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 用户名密码注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult> SignUpWithUserNamePasswordAsync(UserNamePasswordSignUpRequest request)
        {
            var response = new ApiResult();
            try
            {
                var isExist = await _userRepository.TableNotTracking
                    .AnyAsync(item => item.UserName == request.UserName);
                if (isExist)
                {
                    response.Code = Code.Error;
                    response.Message = "该用户名已存在请重新输入";
                    return response;
                }
                var user = new User(true)
                {
                    UserName = request.UserName,
                    CreateTime = DateTime.Now
                };
                var userPassword = new UserPassword(true)
                {
                    UserId = user.Id,
                    PasswordHash = _userPasswordService.PasswordToHash(request.Password),
                    PasswordStrength = request.PasswordStrength,
                    Status = 1,
                    CreateTime = DateTime.Now
                };
                await _userRepository.InsertAsync(user);
                await _userPasswordRepository.InsertAsync(userPassword);
                await _work.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "注册成功";
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"注册异常;method={nameof(SignUpWithUserNamePasswordAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"注册异常：{ex.Message}";
                return response;
            }
        }
    }
}
