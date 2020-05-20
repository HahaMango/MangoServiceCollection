/*--------------------------------------------------------------------------*/
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
using Mango.Service.Blog.Abstractions.Models.Dto;
using Mango.Service.Blog.Abstractions.Models.Entities;
using Mango.Service.Blog.Abstractions.Repositories;
using Mango.Service.Blog.Abstractions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Services
{
    /// <summary>
    /// 文章分类服务实现类
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;

        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        private readonly IEfContextWork _efContextWork;

        public CategoryService(
            ILogger<CategoryService> logger,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IEfContextWork efContextWork)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _efContextWork = efContextWork;
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddCategoryAsync(AddCategoryRequest request,long userId)
        {
            var response = new ApiResult();
            try
            {
                var isExist = await _categoryRepository.TableNotTracking
                    .AnyAsync(item => item.CategoryName == request.CategoryName);
                if (isExist)
                {
                    response.Code = Code.Error;
                    response.Message = "该分类名称以存在";
                    return response;
                }
                var user = await _userRepository.TableNotTracking
                    .FirstOrDefaultAsync(item => item.Id == userId);
                var category = new Category(true)
                {
                    CategoryName = request.CategoryName,
                    Status = 1,
                    CreateTime = DateTime.Now,
                    Creator = user.UserName,
                    UserId = userId,
                    IsDefault = request.IsDefault,
                };
                await _categoryRepository.InsertAsync(category);
                await _efContextWork.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "添加成功";
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"添加分类异常;method={nameof(AddCategoryAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"添加分类异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 查询分类分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageList<CategoryPageResponse>>> QueryCategoryPageAsync(CategoryPageRequest request,long userId)
        {
            var response = new ApiResult<PageList<CategoryPageResponse>>();
            try
            {
                var categories = await (from c in _categoryRepository.TableNotTracking
                                        where c.UserId == userId && c.Status == 1
                                        orderby c.CreateTime descending
                                        select new CategoryPageResponse
                                        {
                                            Id = c.Id,
                                            CategoryName = c.CategoryName
                                        }).ToPageListAsync(request.PageParm.Page, request.PageParm.Size);
                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = categories;
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"查询分类列表异常;method={nameof(QueryCategoryPageAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询分类列表异常：{ex.Message}";
                return response;
            }
        }
    }
}
