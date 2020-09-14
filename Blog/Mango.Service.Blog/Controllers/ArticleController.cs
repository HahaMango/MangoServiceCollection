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
    [Route("api/blog/")]
    public class ArticleController : MangoUserApiController
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        #region 后台接口

        /// <summary>
        /// 后台添加文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = "admin")]
        [HttpPost("admin/article/add")]
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
        /// 后台 查询文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = "admin")]
        [HttpPost("admin/article/detail")]
        public async Task<ApiResult<AdminArticleDetailResponse>> QueryAdminArticleDetailAsync(AdminArticleDetailRequest request)
        {
            var user = GetUser();
            if (user == null)
            {
                return AuthorizeError<AdminArticleDetailResponse>();
            }

            return await _articleService.QueryAdminArticleDetailAsync(request);
        }

        /// <summary>
        /// 后台 编辑文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = "admin")]
        [HttpPost("admin/article/edit")]
        public async Task<ApiResult> AdminEditArticleAsync(AdminEditArticleRequest request)
        {
            var user = GetUser();
            if (user == null)
            {
                return AuthorizeError();
            }

            return await _articleService.AdminEditArticleAsync(request, user.UserId, user.UserName);
        }

        /// <summary>
        /// 后台 删除文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = "admin")]
        [HttpPost("admin/article/delete")]
        public async Task<ApiResult> AdminDeleteArticleAsync(AdminDeleteArticleRequest request)
        {
            var user = GetUser();
            if (user == null)
            {
                return AuthorizeError();
            }

            return await _articleService.AdminDeleteArticleAsync(request, user.UserId, user.UserName);
        }

        #endregion

        #region 前端接口

        /// <summary>
        /// 查询文章分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("article/page")]
        public async Task<ApiResult<PageList<ArticlePageListResponse>>> QueryArticlePageAsync([FromBody]ArticlePageRequest request)
        {
            return await _articleService.QueryArticlePageAsync(request);
        }

        /// <summary>
        /// 查询文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("article/detail")]
        public async Task<ApiResult<ArticleDetailResponse>> QueryArticleDetailAsync([FromBody]ArticleDetailRequest request)
        {
            return await _articleService.QueryArticleDetailAsync(request);
        }

        /// <summary>
        /// 点赞或取消点赞
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("article/like")]
        public async Task<ApiResult> ArticleLikeAsync([FromBody]ArticleLikeRequest request)
        {
            return await _articleService.ArticleLikeAsync(request);
        }

        /// <summary>
        /// 增加阅读数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("article/incview")]
        public async Task<ApiResult> ArticleIncViewAsync([FromBody]IncArticleViewRequest request)
        {
            return await _articleService.IncViewAsync(request);
        }

        #endregion
    }
}