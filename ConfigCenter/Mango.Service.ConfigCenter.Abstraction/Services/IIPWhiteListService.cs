﻿/*--------------------------------------------------------------------------
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
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Service.ConfigCenter.Abstraction.Services
{
    /// <summary>
    /// ip白名单服务接口
    /// </summary>
    public interface IIPWhiteListService
    {
        /// <summary>
        /// 查询允许的Ip列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<string>>> QueryVaildIPList();

        /// <summary>
        /// 指定IP是否匹配
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        Task<ApiResult> IsMatchAsync(string ip);
    }
}
