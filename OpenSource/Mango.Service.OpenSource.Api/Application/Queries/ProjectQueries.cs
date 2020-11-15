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

using Dapper;
using FreeRedis;
using Mango.Core.Dapper;
using Mango.Core.DataStructure;
using Mango.Service.Infrastructure.Helper;
using Mango.Service.OpenSource.Infrastructure.Config;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.OpenSource.Api.Application.Queries
{
    /// <summary>
    /// 项目查询接口实现类
    /// </summary>
    public class ProjectQueries : IProjectQueries
    {
        private readonly ILogger<ProjectQueries> _logger;
        private readonly IDapperHelper _dapperHelper;
        private readonly RedisClient _redisClient;

        public ProjectQueries(
            ILogger<ProjectQueries> logger,
            IDapperHelper dapper,
            RedisClient redisClient)
        {
            _logger = logger;
            _dapperHelper = dapper;
            _redisClient = redisClient;
        }

        /// <summary>
        /// 查询开源项目详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QueryProjectResponseDto> QueryProjectDetailAsync(long id)
        {
            var key = $"{CacheKeyConfig.ProjectDetail}:{id}";
            if (await _redisClient.ExistsAsync(key))
            {
                return await _redisClient.GetAsync<QueryProjectResponseDto>(key);
            }
            var sql = $@"
            select * 
            from project
            where Id = {id}
            ";

            var result = await _dapperHelper.QueryFirstOrDefaultAsync<QueryProjectResponseDto>(sql);
            await _redisClient.SetAsync(key, result, DateHelper.SecondOfDay(7));
            return result;
        }

        /// <summary>
        /// 查询开源项目分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PageList<QueryProjectResponseDto>> QueryProjectPageAsync(QueryProjectPageRequestDto request)
        {
            var sql = $@"
            select * 
            from project
            where UserId = {request.UserId} and Status = 1
            order by SortId desc, CreateTime desc
            limit {(request.PageParm.Page - 1) * request.PageParm.Size}, {request.PageParm.Size}
            ";

            var countSql = $@"
            select count(*)
            from project
            where UserId = {request.UserId} and Status = 1
            ";

            var result = await _dapperHelper.QueryAsync<QueryProjectResponseDto>(sql);
            var count = await _dapperHelper.QueryFirstAsync<int>(countSql);
            var pagerlist = new PageList<QueryProjectResponseDto>(request.PageParm.Page, request.PageParm.Size, count, result);

            return pagerlist;
        }
    }
}
