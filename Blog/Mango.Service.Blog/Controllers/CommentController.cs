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
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Controllers
{
    /// <summary>
    /// 评论controller
    /// </summary>
    public class CommentController: MangoUserApiController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// 添加文章评论
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/comment/add")]
        public async Task<ApiResult> AddCommentAsync([FromBody]CommentRequest request)
        {
            var userId = default(long?);
            var user = GetUser();
            if(user != null)
            {
                userId = user.UserId;
            }
            return await _commentService.ArticleCommentAsync(request, userId);
        }

        /// <summary>
        /// 添加文章子评论
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddReplyCommentAsync([FromBody]CommentReplyRequest request)
        {
            var userId = default(long?);
            var user = GetUser();
            if(user != null)
            {
                userId = user.UserId;
            }
            return await _commentService.ArticleSubCommentAsync(request, userId);
        }

        /// <summary>
        /// 查询文章评论分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/comment/page")]
        public async Task<ApiResult<PageList<CommentPageResponse>>> QueryCommentPageAsync([FromBody]CommentPageRequest request)
        {
            return await _commentService.QueryCommentPageAsync(request, null);
        }
    }
}
