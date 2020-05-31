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
using Mango.Core.Enums;
using Mango.Service.ConfigCenter.Abstraction.Models.Entities;
using Mango.Service.ConfigCenter.Abstraction.Repositories;
using Mango.Service.ConfigCenter.Abstraction.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.ConfigCenter.Services
{
    /// <summary>
    /// 全局配置服务接口实现
    /// </summary>
    public class GlobalConfigService : IGlobalConfigService
    {
        private readonly ILogger<GlobalConfigService> _logger;

        private readonly IGlobalConfigRepository _globalConfigRepository;

        public GlobalConfigService(
            ILogger<GlobalConfigService> logger,
            IGlobalConfigRepository globalConfigRepository)
        {
            _logger = logger;
            _globalConfigRepository = globalConfigRepository;
        }

        /// <summary>
        /// 查询全局配置
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<GlobalConfig>> QueryGlobalConfigAsync()
        {
            var response = new ApiResult<GlobalConfig>();
            try
            {
                var config = await _globalConfigRepository.TableNotTracking
                    .FirstOrDefaultAsync(item => item.Status == 1);

                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = config;
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"添加模块异常;method={nameof(QueryGlobalConfigAsync)};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"添加模块异常：{ex.Message}";
                return response;
            }
        }
    }
}
