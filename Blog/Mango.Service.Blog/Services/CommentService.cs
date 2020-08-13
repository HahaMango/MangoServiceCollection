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
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Services
{
    /// <summary>
    /// 评论服务实现类
    /// </summary>
    public class CommentService : ICommentService
    {
        private readonly ILogger<CommentService> _logger;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEfContextWork _efContextWork;

        public CommentService(
            ILogger<CommentService> logger
            , ICommentRepository commentRepository
            , IUserRepository userRepository
            , IEfContextWork efContextWork)
        {
            _logger = logger;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _efContextWork = efContextWork;
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult> ArticleCommentAsync(CommentRequest request, long? userId)
        {
            var response = new ApiResult();
            try
            {
                var comment = new Comment(true)
                {
                    ArticleId = request.ArticleId,
                    UserId = userId,
                    UserName = request.UserName,
                    Content = request.Content,
                    CreateTime = DateTime.Now,
                    Status = 1
                };
                await _commentRepository.InsertAsync(comment);
                await _efContextWork.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "评论成功";
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"添加评论异常;method={nameof(ArticleCommentAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"添加评论异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 回复评论
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult> ArticleSubCommentAsync(CommentReplyRequest request, long? userId)
        {
            var response = new ApiResult();
            try
            {
                var comment = new Comment(true)
                {
                    ArticleId = request.ArticleId,
                    UserId = userId,
                    UserName = request.UserName,
                    Content = request.Content,
                    CreateTime = DateTime.Now,
                    IsReply = 1,
                    ReplyCommentId = request.ReplyCommentId,
                    Status = 1
                };
                if (request.ReplySubCommentId.HasValue)
                {
                    comment.IsSubReply = 1;
                    comment.ReplySubCommentId = request.ReplySubCommentId.Value;
                    comment.ReplySubUserId = request.ReplySubUserId;
                    comment.ReplySubUserName = request.ReplySubUserName;
                }
                await _commentRepository.InsertAsync(comment);
                await _efContextWork.SaveChangesAsync();

                response.Code = Code.Ok;
                response.Message = "评论成功";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"添加回复评论异常;method={nameof(ArticleCommentAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"添加回复评论异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 查询评论分页数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageList<CommentPageResponse>>> QueryCommentPageAsync(CommentPageRequest request, long? userId)
        {
            var response = new ApiResult<PageList<CommentPageResponse>>();
            try
            {
                var comments = await (from c in _commentRepository.TableNotTracking
                                      where c.ArticleId == request.ArticleId && c.Status == 1
                                      orderby c.CreateTime descending
                                      select new CommentPageResponse
                                      {
                                          Id = c.Id,
                                          ArticleId = c.ArticleId,
                                          UserId = c.UserId,
                                          Content = c.Content,
                                          Like = c.Like,
                                          Reply = c.Reply,
                                          UserName = c.UserName
                                      }).ToPageListAsync(request.PageParm.Page, request.PageParm.Size);

                foreach(var c in comments.Data)
                {
                    var replyComments = await (from rc in _commentRepository.TableNotTracking
                                               where rc.IsReply == 1 && rc.ReplyCommentId == c.Id && rc.Status == 1
                                               orderby rc.CreateTime descending
                                               select new CommentSubPageResponse
                                               {
                                                   Id = c.Id,
                                                   ArticleId = c.ArticleId,
                                                   UserId = c.UserId,
                                                   Content = c.Content,
                                                   Like = c.Like,
                                                   Reply = c.Reply,
                                                   UserName = c.UserName,
                                                   CreateTime = c.CreateTime,
                                                   SubReplyUserId = rc.ReplySubUserId,
                                                   SubReplyUserName = rc.ReplySubUserName
                                               })
                                               .FirstOrDefaultAsync();
                    c.FirstReplyComment = replyComments;
                }

                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = comments;
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"查询评论分页异常;method={nameof(ArticleCommentAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询评论分页异常：{ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 查询子评论分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageList<CommentSubPageResponse>>> QuerySubCommentPageAsync(SubCommentPageRequest request, long? userId)
        {
            var response = new ApiResult<PageList<CommentSubPageResponse>>();
            try
            {
                var subComments = await (from c in _commentRepository.TableNotTracking
                                         where c.Reply == 1 && c.ReplyCommentId == request.CommentId && c.Status == 1
                                         orderby c.CreateTime descending
                                         select new CommentSubPageResponse
                                         {
                                             Id = c.Id,
                                             ArticleId = c.ArticleId,
                                             UserId = c.UserId,
                                             SubReplyUserId = c.ReplySubUserId,
                                             UserName = c.UserName,
                                             SubReplyUserName = c.ReplySubUserName,
                                             SubReplyCommentId = c.ReplySubCommentId,
                                             Content = c.Content,
                                             Like = c.Like,
                                             Reply = c.Reply,
                                             CreateTime = c.CreateTime
                                         })
                                         .ToPageListAsync(request.PageParm.Page, request.PageParm.Size);

                response.Code = Code.Ok;
                response.Message = "查询成功";
                response.Data = subComments;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"查询子评论分页异常;method={nameof(QuerySubCommentPageAsync)};param={request.ToJson()};exception messges={ex.Message}");
                response.Code = Code.Error;
                response.Message = $"查询子评论分页异常：{ex.Message}";
                return response;
            }
        }
    }
}
