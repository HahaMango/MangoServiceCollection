﻿/*--------------------------------------------------------------------------*/
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
using Mango.Service.Blog.Abstractions.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Abstractions.Services
{
    /// <summary>
    /// 文章分类服务接口
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult> AddCategoryAsync(AddCategoryRequest request,long userId);

        /// <summary>
        /// 查询分类分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<PageList<CategoryPageResponse>>> QueryCategoryPageAsync(CategoryPageRequest request,long userId); 
    }
}
