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
using Mango.Core.Serialization.Extension;
using Mango.EntityFramework.Abstractions;
using Mango.Service.ConfigCenter.Abstraction.Models.Dto;
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
    /// 模块服务接口实现
    /// </summary>
    public class ModuleService : IModuleService
    {
        private readonly ILogger<ModuleService> _logger;

        private readonly IModuleRepository _moduleRepository;
        private readonly IEfContextWork _work;

        public ModuleService(
            ILogger<ModuleService> logger,
            IModuleRepository moduleRepository,
            IEfContextWork work)
        {
            _logger = logger;
            _moduleRepository = moduleRepository;
            _work = work;
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddModuleAsync(AddModuleRequest request, long userId)
        {
            var response = new ApiResult();
            try
            {
                var isExist = await _moduleRepository.TableNotTracking
                    .AnyAsync(item => item.Status == 1 && item.ModuleName == request.ModuleName);
                if (isExist)
                {
                    response.Code = Code.Error;
                    response.Message = "模块已存在";
                    return response;
                }
                var module = new Module(true)
                {
                    ModuleName = request.ModuleName,
                    ModuleSecret = request.ModuleSecret,
                    Status = 1,
                    CreateTime = DateTime.Now
                };

                await _moduleRepository.InsertAsync(module);
                await _work.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "添加成功";
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"添加模块异常;method={nameof(AddModuleAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"添加模块异常：{ex.Message}";
                return response;
            }
        }
    }
}
