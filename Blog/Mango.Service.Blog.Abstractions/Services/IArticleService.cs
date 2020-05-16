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
    /// 文章服务接口
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult> AddArticleAsync(AddArticleRequest request, long userId);

        /// <summary>
        /// 查询文章分页数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<PageList<ArticlePageListResponse>>> QueryArticlePageAsync(ArticlePageRequest request);

        /// <summary>
        /// 查询文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult<ArticleDetailResponse>> QueryArticleDetailAsync(ArticleDetailRequest request);

        /// <summary>
        /// 文章点赞
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResult> ArticleLikeAsync(ArticleLikeRequest request);
    }
}
