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
using Mango.Core.DataStructure;
using Mango.Service.OpenSource.Abstraction.Models.Dto;
using Mango.Service.OpenSource.Abstraction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.OpenSource.Services
{
    /// <summary>
    /// 开源仓储服务接口实现
    /// </summary>
    public class OpenSourceService : IOpenSourceService
    {
        public Task<ApiResult> AddOpenSourceProjectAdminAsync(AddOpenSourceProjectAdminRequest request, long userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult> DeleteOpenSourceProjectAdminAsync(DeleteOpenSourceProjectAdminRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult> EditOpenSourceProjectAdminAsync(EditOpenSourceProjectAdminRequest request, long userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<PageList<QueryOpenSourceProjectAdminResponse>>> QueryOpenSourcePageAdminAsync(QueryOpenSourcePageAdminRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<QueryOpenSourceProjectResponse>> QueryOpenSourcePageAsync(QueryOpenSourcePageRequest request, long userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<QueryOpenSourceProjectAdminResponse>> QueryOpenSourceProjectAdminAsync(QueryOpenSourceProjectAdminRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<QueryOpenSourceProjectResponse>> QueryOpenSourceProjectAsync(QueryOpenSourceProjectRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
