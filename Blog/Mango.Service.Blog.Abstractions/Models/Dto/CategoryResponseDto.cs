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

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mango.Service.Blog.Abstractions.Models.Dto
{
    public class CategoryResponseDto
    {

    }

    #region 类目分类列表响应类

    public class CategoryPageResponse
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        [JsonPropertyName("CategoryId")]
        public long Id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
    }

    #endregion
}
