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
using Mango.Service.Blog.Abstractions.Models.Dto;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Abstractions.Services
{
    /// <summary>
    /// 评论服务接口
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// 文章评论
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId">当前评论用户的Id,如果未登陆为0</param>
        /// <returns></returns>
        Task<ApiResult<CommentPageResponse>> ArticleCommentAsync(CommentRequest request, long? userId);

        /// <summary>
        /// 文章回复
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult<CommentSubPageResponse>> ArticleSubCommentAsync(CommentReplyRequest request, long? userId);

        /// <summary>
        /// 查询文章评论分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult<PageList<CommentPageResponse>>> QueryCommentPageAsync(CommentPageRequest request, long? userId);

        /// <summary>
        /// 查询子评论分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult<PageList<CommentSubPageResponse>>> QuerySubCommentPageAsync(SubCommentPageRequest request, long? userId);
    }
}
