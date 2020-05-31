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
using Mango.Core.Extension;
using Mango.Core.Serialization.Extension;
using Mango.EntityFramework.Abstractions;
using Mango.Service.ConfigCenter.Abstraction.Models.Dto;
using Mango.Service.ConfigCenter.Abstraction.Models.Entities;
using Mango.Service.ConfigCenter.Abstraction.Repositories;
using Mango.Service.ConfigCenter.Abstraction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.ConfigCenter.Services
{
    /// <summary>
    /// 模块配置服务接口实现
    /// </summary>
    public class ModuleConfigService : IModuleConfigService
    {
        private readonly ILogger<ModuleConfigService> _logger;

        private readonly IModuleConfigRepository _moduleConfigRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IEfContextWork _work;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ModuleConfigService(
            ILogger<ModuleConfigService> logger,
            IModuleConfigRepository moduleConfigRepository,
            IModuleRepository moduleRepository,
            IHttpContextAccessor httpContextAccessor,
            IEfContextWork work)
        {
            _logger = logger;
            _moduleConfigRepository = moduleConfigRepository;
            _moduleRepository = moduleRepository;
            _httpContextAccessor = httpContextAccessor;
            _work = work;
        }

        /// <summary>
        /// 查询模块配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<ApiResult<ModuleConfigResponse>> QueryModuleConfigAsync(QueryModuleConfigRequest request)
        {
            var response = new ApiResult<ModuleConfigResponse>();
            try
            {
                var module = await _moduleRepository.Table
                    .FirstOrDefaultAsync(item => item.ModuleName == request.ModuleName && item.Status == 1);
                if(module == null)
                {
                    response.Code = Code.Error;
                    response.Message = "模块不存在";
                    return response;
                }
                var moduleConfig = await _moduleConfigRepository.TableNotTracking
                    .FirstOrDefaultAsync(item => item.ModuleId == module.Id && item.Status == 1);
                var moduleConfigResponse = moduleConfig.MapTo<ModuleConfigResponse>();
                moduleConfigResponse.ModuleName = module.ModuleName;
                moduleConfigResponse.ModuleSecret = module.ModuleSecret;
                moduleConfigResponse.Id = module.Id;

                module.LastLoadTime = DateTime.Now;
                module.LastLoadIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                await _work.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = moduleConfigResponse;
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"查询模块配置异常;method={nameof(QueryModuleConfigAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询模块配置异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 添加模块配置
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddModuleConfigAsync(AddModuleConfigRequest request, long userId)
        {
            var response = new ApiResult();
            try
            {
                var module = await _moduleRepository.TableNotTracking
                    .FirstOrDefaultAsync(item => item.ModuleName == request.ModuleName && item.Status == 1);
                if(module == null)
                {
                    response.Code = Code.Error;
                    response.Message = "模块不存在";
                    return response;
                }
                var moduleConfig = request.MapTo<ModuleConfig>();
                moduleConfig.SetId();
                moduleConfig.ModuleId = module.Id;
                moduleConfig.CreateTime = DateTime.Now;
                moduleConfig.Status = 1;
                #region 失效以前的配置
                var oldConfig = await _moduleConfigRepository.Table
                    .Where(item => item.ModuleId == module.Id && item.Status == 1)
                    .ToListAsync();
                oldConfig.ForEach(i =>
                {
                    i.Status = 0;
                    i.UpdateTime = DateTime.Now;
                });
                #endregion
                await _moduleConfigRepository.InsertAsync(moduleConfig);
                await _work.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "添加成功";
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"添加模块配置异常;method={nameof(AddModuleConfigAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"添加模块配置异常：{ex.Message}";
                return response;
            }
        }
    }
}
