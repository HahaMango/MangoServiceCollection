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
using Mango.Core.Encryption;
using Mango.Core.Enums;
using Mango.Core.Serialization.Extension;
using Mango.EntityFramework.Abstractions;
using Mango.Infrastructure.Helper;
using Mango.Service.UserCenter.Abstraction.Models.Dto;
using Mango.Service.UserCenter.Abstraction.Models.Entities;
using Mango.Service.UserCenter.Abstraction.Repositories;
using Mango.Service.UserCenter.Abstraction.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.UserCenter.Services
{
    /// <summary>
    /// 用户密码服务接口实现
    /// </summary>
    public class UserPasswordService : IUserPasswordService
    {
        private readonly ILogger<UserPasswordService> _logger;
        private readonly IUserPasswordRepository _userPasswordRepository;
        private readonly IUserRepository _userRepository;

        private readonly IEfContextWork _work;

        public UserPasswordService(
            ILogger<UserPasswordService> logger,
            IUserPasswordRepository userPasswordRepository,
            IUserRepository userRepository,
            IEfContextWork work)
        {
            _logger = logger;
            _userPasswordRepository = userPasswordRepository;
            _userRepository = userRepository;
            _work = work;
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult> ChangePasswordAsync(ChangePasswordRequest request, long userId)
        {
            var response = new ApiResult();
            try
            {
                var user = await _userRepository.Table
                    .FirstOrDefaultAsync(item => item.Id == userId && item.Status == 1);

                #region 验证
                if (user == null)
                {
                    response.Code = Code.Error;
                    response.Message = "用户不存在";
                    return response;
                }
                if(!await VerifyPasswordAsync(userId,request.OldPassword))
                {
                    response.Code = Code.Error;
                    response.Message = "密码错误";
                    return response;
                }
                if (request.Password != request.PasswordConfirm)
                {
                    response.Code = Code.Error;
                    response.Message = "确认密码错误，请重试";
                    return response;
                }
                #endregion

                var userPasswords = await _userPasswordRepository.Table
                    .Where(item => item.UserId == userId)
                    .ToListAsync();

                #region 验证是否以前设置过的密码
                var newHash = PasswordToHash(request.Password);
                foreach(var password in userPasswords)
                {
                    if(password.PasswordHash == newHash)
                    {
                        response.Code = Code.Error;
                        response.Message = "以前已设置过该密码，请设置新的密码";
                        return response;
                    }
                }
                #endregion

                var newPassword = new UserPassword(true)
                {
                    UserId = userId,
                    PasswordHash = newHash,
                    PasswordStrength = request.PasswordStrength,
                    Status = 1,
                    CreateTime = DateTime.Now
                };
                userPasswords.ForEach(item =>
                {
                    item.Status = 0;
                });
                user.UpdateTime = DateTime.Now;
                await _userPasswordRepository.InsertAsync(newPassword);
                await _work.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"更改密码异常;method={nameof(ChangePasswordAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"更改密码异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> VerifyPasswordAsync(long userId, string password)
        {
            var userPassword = await _userPasswordRepository.TableNotTracking
                .FirstOrDefaultAsync(item => item.UserId == userId && item.Status == 1);

            if (PasswordToHash(password) != userPassword.PasswordHash)
                return false;
            return true;
        }

        /// <summary>
        /// 生成秘密散列
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string PasswordToHash(string password)
        {
            return StringHelper.ToHex(MangoSHA256.Encrypt(password));
        }
    }
}
