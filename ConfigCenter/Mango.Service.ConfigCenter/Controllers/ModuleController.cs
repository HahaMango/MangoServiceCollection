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
using Mango.Service.ConfigCenter.Abstraction.Models.Dto;
using Mango.Service.ConfigCenter.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.ConfigCenter.Controllers
{
    /// <summary>
    /// 模块controller
    /// </summary>
    public class ModuleController : MangoUserApiController
    {
        private readonly IModuleService _moduleService;
        private readonly IModuleConfigService _moduleConfigService;

        public ModuleController(
            IModuleService moduleService,
            IModuleConfigService moduleConfigService)
        {
            _moduleService = moduleService;
            _moduleConfigService = moduleConfigService;
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/configcenter/addmodule")]
        public async Task<ApiResult> AddModuleAsync([FromBody]AddModuleRequest request)
        {
            return await _moduleService.AddModuleAsync(request, 0);
        }

        /// <summary>
        /// 添加模块配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/configcenter/addconfig")]
        public async Task<ApiResult> AddConfigAsync([FromBody]AddModuleConfigRequest request)
        {
            return await _moduleConfigService.AddModuleConfigAsync(request, 0);
        }

        /// <summary>
        /// 查询模块配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/configcenter/queryconfig")]
        public async Task<ApiResult<ModuleConfigResponse>> QueryConfigAsync([FromBody]QueryModuleConfigRequest request)
        {
            return await _moduleConfigService.QueryModuleConfigAsync(request);
        }
    }
}
