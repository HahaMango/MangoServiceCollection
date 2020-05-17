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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Core.ApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mango.Core.Enums;

namespace Mango.Service.Blog.Controllers
{
    /// <summary>
    /// 文章Controller
    /// </summary>
    [ApiController]
    public class ArticleController : ControllerBase
    {
        [HttpGet("api/article/test")]
        public async Task<ApiResult<string>> Test()
        {
            return await Task.FromResult(new ApiResult<string>
            {
                Code = Code.OK,
                Message = "查询成功",
                Data = "data"
            });
        }
    }
}