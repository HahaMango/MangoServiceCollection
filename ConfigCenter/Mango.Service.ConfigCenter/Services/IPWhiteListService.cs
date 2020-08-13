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
    /// ip白名单服务实现（Startup.cs里面被管道中间件使用）
    /// 
    /// 后期性能不好再考虑改用redis
    /// </summary>
    public class IPWhiteListService : IIPWhiteListService
    {
        private readonly ILogger<IPWhiteListService> _logger;

        private readonly IIPWhiteListRepository _iPWhiteListRepository;

        public IPWhiteListService(
            ILogger<IPWhiteListService> logger,
            IIPWhiteListRepository iPWhiteListRepository)
        {
            _logger = logger;
            _iPWhiteListRepository = iPWhiteListRepository;
        }

        /// <summary>
        /// 指定Ip是否匹配（暂时模糊匹配，以后改掩码匹配）
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public async Task<ApiResult> IsMatchAsync(string ip)
        {
            var response = new ApiResult();
            try
            {
                var isAccept = await _iPWhiteListRepository.TableNotTracking
                    .AnyAsync(item => item.IP == ip);
                if (!isAccept)
                {
                    if (ip.Contains('.'))
                    {
                        var ipArray = ip.Split('.');
                        ip = $"{ipArray[0]}.{ipArray[1]}.{ipArray[2]}";
                        isAccept = await _iPWhiteListRepository.TableNotTracking
                            .AnyAsync(item => item.IP.Contains(ip));
                        if (!isAccept)
                        {
                            response.Code = Code.Error;
                            response.Message = "匹配失败";
                            return response;
                        }
                    }
                }
                response.Code = Code.Ok;
                response.Message = "匹配成功";
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"IP白名单匹配异常;method={nameof(QueryVaildIPList)};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"IP白名单匹配异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 查询允许的ip地址
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<List<string>>> QueryVaildIPList()
        {
            var response = new ApiResult<List<string>>();
            try
            {
                var ips = await _iPWhiteListRepository.TableNotTracking
                    .Select(item => item.IP)
                    .ToListAsync();
                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = ips;
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"查询ip白名单异常;method={nameof(QueryVaildIPList)};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询ip白名单异常：{ex.Message}";
                return response;
            }
        }
    }
}
