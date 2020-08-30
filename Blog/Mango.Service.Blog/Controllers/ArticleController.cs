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
using System.Threading.Tasks;

namespace Mango.Service.Blog.Controllers
{
    /// <summary>
    /// 文章Controller
    /// </summary>
    public class ArticleController : MangoUserApiController
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = "admin")]
        [HttpPost("api/article/add")]
        public async Task<ApiResult> AddArticleAsync([FromBody]AddArticleRequest request)
        {
            var user = GetUser();
            if (user == null)
            {
                return AuthorizeError();
            }
            return await _articleService.AddArticleAsync(request, user.UserId);
        }

        /// <summary>
        /// 查询文章分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/article/page")]
        [AllowAnonymous]
        public async Task<ApiResult<PageList<ArticlePageListResponse>>> QueryArticlePageAsync([FromBody]ArticlePageRequest request)
        {
            return await _articleService.QueryArticlePageAsync(request);
        }

        /// <summary>
        /// 查询文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/article/detail")]
        [AllowAnonymous]
        public async Task<ApiResult<ArticleDetailResponse>> QueryArticleDetailAsync([FromBody]ArticleDetailRequest request)
        {
            return await _articleService.QueryArticleDetailAsync(request);
        }

        /// <summary>
        /// 点赞或取消点赞
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/article/like")]
        [AllowAnonymous]
        public async Task<ApiResult> ArticleLikeAsync([FromBody]ArticleLikeRequest request)
        {
            return await _articleService.ArticleLikeAsync(request);
        }

        /// <summary>
        /// 增加阅读数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/article/incview")]
        [AllowAnonymous]
        public async Task<ApiResult> ArticleIncViewAsync([FromBody]IncArticleViewRequest request)
        {
            return await _articleService.IncViewAsync(request);
        }
    }
}