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

using Mango.Core.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mango.Service.OpenSource.Api.Application.Queries
{
    /// <summary>
    /// 项目查询接口
    /// </summary>
    public interface IProjectQueries
    {
        /// <summary>
        /// 查询开源项目分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageList<QueryProjectResponseDto>> QueryProjectPageAsync(QueryProjectPageRequestDto request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查询开源项目详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QueryProjectResponseDto> QueryProjectDetailAsync(long id, CancellationToken cancellationToken = default);
    }
}
