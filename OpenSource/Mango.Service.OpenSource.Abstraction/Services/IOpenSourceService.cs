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
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Service.OpenSource.Abstraction.Services
{
    /// <summary>
    /// 开源仓库服务接口
    /// </summary>
    public interface IOpenSourceService
    {
        #region 后台
        /// <summary>
        /// 添加开源仓库
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult> AddOpenSourceProjectAdminAsync(AddOpenSourceProjectAdminRequest request,long userId);

        /// <summary>
        /// 编辑开源仓库
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult> EditOpenSourceProjectAdminAsync(EditOpenSourceProjectAdminRequest request, long userId);

        /// <summary>
        /// 删除开源仓库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult> DeleteOpenSourceProjectAdminAsync(DeleteOpenSourceProjectAdminRequest request);

        /// <summary>
        /// 查询开源详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<QueryOpenSourceProjectAdminResponse>> QueryOpenSourceProjectAdminAsync(QueryOpenSourceProjectAdminRequest request);

        /// <summary>
        /// 查询开源项目分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<PageList<QueryOpenSourceProjectAdminResponse>>> QueryOpenSourcePageAdminAsync(QueryOpenSourcePageAdminRequest request);
        #endregion

        #region web端
        /// <summary>
        /// 查询开源仓库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<QueryOpenSourceProjectResponse>> QueryOpenSourceProjectAsync(QueryOpenSourceProjectRequest request);

        /// <summary>
        /// 查询开源项目分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult<PageList<QueryOpenSourceProjectResponse>>> QueryOpenSourcePageAsync(QueryOpenSourcePageRequest request,long userId);
        #endregion
    }
}
