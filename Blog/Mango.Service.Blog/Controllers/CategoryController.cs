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
using Mango.Core.DataStructure;
using Mango.Service.Blog.Abstractions.Models.Dto;
using Mango.Service.Blog.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Controllers
{
    /// <summary>
    /// 文章类别controller
    /// </summary>
    [Authorize]
    public class CategoryController : MangoUserApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// 添加文章分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/category/add")]
        public async Task<ApiResult> AddCategoryAsync([FromBody]AddCategoryRequest request)
        {
            var user = GetUser();
            if(user == null)
            {
                return AuthorizeError();
            }
            return await _categoryService.AddCategoryAsync(request, user.UserId);
        }

        /// <summary>
        /// 查询用户文章类别分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/category/page")]
        public async Task<ApiResult<PageList<CategoryPageResponse>>> QueryCategoryPageAsync([FromBody]CategoryPageRequest request)
        {
            var user = GetUser();
            if(user == null)
            {
                return AuthorizeError<PageList<CategoryPageResponse>>();
            }
            return await _categoryService.QueryCategoryPageAsync(request, user.UserId);
        }
    }
}
