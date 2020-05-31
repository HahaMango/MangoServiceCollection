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
using Mango.Service.ConfigCenter.Abstraction.Models.Entities;
using Mango.Service.ConfigCenter.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.ConfigCenter.Controllers
{
    /// <summary>
    /// 全局配置controller
    /// </summary>
    public class GlobalConfigController : MangoUserApiController
    {
        private readonly IGlobalConfigService _globalConfigService;

        public GlobalConfigController(
            IGlobalConfigService globalConfigService)
        {
            _globalConfigService = globalConfigService;
        }

        /// <summary>
        /// 查询全局配置
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/configcenter/global")]
        public async Task<ApiResult<GlobalConfig>> QueryGlobalConfigAsync()
        {
            return await _globalConfigService.QueryGlobalConfigAsync();
        }
    }
}
