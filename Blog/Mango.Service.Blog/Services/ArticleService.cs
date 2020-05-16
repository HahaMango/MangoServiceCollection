using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Core.ApiResponse;
using Mango.Core.DataStructure;
using Mango.Service.Blog.Abstractions.Models.Dto;
using Mango.Service.Blog.Abstractions.Services;

namespace Mango.Service.Blog.Services
{
    /// <summary>
    /// 文章服务实现类
    /// </summary>
    public class ArticleService : IArticleService
    {
        public Task<ApiResult> AddArticleAsync(AddArticleRequest request, long userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult> ArticleLikeAsync(ArticleLikeRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<ArticleDetailResponse>> QueryArticleDetailAsync(ArticleDetailRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<PageList<ArticlePageListResponse>>> QueryArticlePageAsync(ArticlePageRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
