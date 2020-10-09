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
using Mango.Core.Enums;
using Mango.Core.Extension;
using Mango.Core.Serialization.Extension;
using Mango.EntityFramework.Abstractions;
using Mango.Service.OpenSource.Abstraction.Models.Dto;
using Mango.Service.OpenSource.Abstraction.Models.Entities;
using Mango.Service.OpenSource.Abstraction.Repositories;
using Mango.Service.OpenSource.Abstraction.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<OpenSourceService> _logger;

        private readonly IOpenSourceProjectRepository _openSourceProjectRepository;
        private readonly IEfContextWork _work;

        public OpenSourceService(
            ILogger<OpenSourceService> logger,
            IOpenSourceProjectRepository openSourceProjectRepository,
            IEfContextWork efContextWork)
        {
            _logger = logger;
            _openSourceProjectRepository = openSourceProjectRepository;
            _work = efContextWork;
        }

        /// <summary>
        /// 添加开源仓库
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddOpenSourceProjectAdminAsync(AddOpenSourceProjectAdminRequest request, long userId)
        {
            var response = new ApiResult();
            try
            {
                var project = request.MapTo<OpenSourceProject>();
                project.SetId();
                project.Status = 1;
                project.CreateTime = DateTime.Now;
                project.UserId = userId;

                await _openSourceProjectRepository.InsertAsync(project);
                await _work.SaveChangesAsync();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"添加开源仓库异常;method={nameof(AddOpenSourceProjectAdminAsync)};param={request?.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"添加开源仓库异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 删除开源仓库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult> DeleteOpenSourceProjectAdminAsync(DeleteOpenSourceProjectAdminRequest request)
        {
            var response = new ApiResult();
            try
            {
                var project = await _openSourceProjectRepository.Table
                    .FirstOrDefaultAsync(item => item.Id == request.Id && item.Status == 1);
                if(project == null)
                {
                    response.Code = Code.Error;
                    response.Message = "查无开源仓库";
                    return response;
                }
                project.Status = 0;
                project.UpdateTime = DateTime.Now;

                await _work.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"删除开源仓库异常;method={nameof(DeleteOpenSourceProjectAdminAsync)};param={request?.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"删除开源仓库异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 编辑开源仓库
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult> EditOpenSourceProjectAdminAsync(EditOpenSourceProjectAdminRequest request, long userId)
        {
            var response = new ApiResult();
            try
            {
                var project = await _openSourceProjectRepository.Table
                    .FirstOrDefaultAsync(item => item.Id == request.Id && item.Status == 1 && item.UserId == userId);
                if (project == null)
                {
                    response.Code = Code.Error;
                    response.Message = "查无开源仓库";
                    return response;
                }
                project.Title = request.Title;
                project.Desc = request.Desc;
                project.RepositoryUrl = request.RepositoryUrl;
                project.Image = request.Image;
                project.README = request.README;
                project.Platform = request.Platform;
                project.UpdateTime = DateTime.Now;

                await _work.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"编辑开源仓库异常;method={nameof(EditOpenSourceProjectAdminAsync)};param={request?.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"编辑开源仓库异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 总后台-查询开源项目分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageList<QueryOpenSourceProjectAdminResponse>>> QueryOpenSourcePageAdminAsync(QueryOpenSourcePageAdminRequest request)
        {
            var response = new ApiResult<PageList<QueryOpenSourceProjectAdminResponse>>();
            try
            {
                var projects = await (from p in _openSourceProjectRepository.TableNotTracking
                                      where p.UserId == request.UserId && p.Status == 1
                                      select p.MapTo<QueryOpenSourceProjectAdminResponse>())
                                      .ToPageListAsync(request.PageParm.Page, request.PageParm.Size);

                response.Data = projects;
                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"总后台-查询开源项目分页异常;method={nameof(QueryOpenSourcePageAdminAsync)};param={request?.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"总后台-查询开源项目分页异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 查询开源项目分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageList<QueryOpenSourceProjectResponse>>> QueryOpenSourcePageAsync(QueryOpenSourcePageRequest request, long userId)
        {
            var response = new ApiResult<PageList<QueryOpenSourceProjectResponse>>();
            try
            {
                var projects = await(from p in _openSourceProjectRepository.TableNotTracking
                                     where p.UserId == request.UserId && p.Status == 1
                                     select p.MapTo<QueryOpenSourceProjectResponse>())
                                      .ToPageListAsync(request.PageParm.Page, request.PageParm.Size);

                response.Data = projects;
                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"查询开源项目分页异常;method={nameof(QueryOpenSourcePageAsync)};param={request?.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询开源项目分页异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 总后台查询开源仓库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<QueryOpenSourceProjectAdminResponse>> QueryOpenSourceProjectAdminAsync(QueryOpenSourceProjectAdminRequest request)
        {
            var response = new ApiResult<QueryOpenSourceProjectAdminResponse>();
            try
            {
                var project = await _openSourceProjectRepository.Table
                    .FirstOrDefaultAsync(item => item.Id == request.Id && item.Status == 1);
                if (project == null)
                {
                    response.Code = Code.Error;
                    response.Message = "查无开源仓库";
                    return response;
                }

                response.Data = project.MapTo<QueryOpenSourceProjectAdminResponse>();
                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"总后台查询开源仓库异常;method={nameof(QueryOpenSourceProjectAdminAsync)};param={request?.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"总后台查询开源仓库异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 查询开源仓库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<QueryOpenSourceProjectResponse>> QueryOpenSourceProjectAsync(QueryOpenSourceProjectRequest request)
        {
            var response = new ApiResult<QueryOpenSourceProjectResponse>();
            try
            {
                var project = await _openSourceProjectRepository.Table
                    .FirstOrDefaultAsync(item => item.Id == request.Id && item.Status == 1);
                if (project == null)
                {
                    response.Code = Code.Error;
                    response.Message = "查无开源仓库";
                    return response;
                }

                response.Data = project.MapTo<QueryOpenSourceProjectResponse>();
                response.Code = Code.Ok;
                response.Message = "操作成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"查询开源仓库异常;method={nameof(QueryOpenSourceProjectAsync)};param={request?.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询开源仓库异常：{ex.Message}";
                return response;
            }
        }
    }
}
