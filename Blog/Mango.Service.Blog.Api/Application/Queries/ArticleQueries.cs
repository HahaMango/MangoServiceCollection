﻿/*--------------------------------------------------------------------------
//
//  Copyright 2021 Chiva Chen
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
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Mango.Core.Dapper;
using Mango.Core.DataStructure;
using Mango.Service.Blog.Domain.AggregateModel.ArticleDataAggregate;
using Mango.Service.Blog.Infrastructure.Repositories;

namespace Mango.Service.Blog.Api.Application.Queries
{
    /// <summary>
    /// 文章查询实现（职责分离）
    /// </summary>
    public class ArticleQueries : IArticleQueries
    {
        private readonly IDapperHelper _dapperHelper;

        private readonly IArticleDataRepository _articleDataRepository;

        public ArticleQueries(IDapperHelper dapperHelper, ArticleDataRepository articleDataRepository)
        {
            _dapperHelper = dapperHelper;
            _articleDataRepository = articleDataRepository;
        }
        
        /// <summary>
        /// 查询文章分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PageList<QueryArticlePageResponse>> QueryArticlePageAsync(QueryArticlePageRequest request, CancellationToken cancellationToken = default)
        {
            var sql = "select * from article where BloggerId=@BloggerId limit @Skip,@Take";
            var param = new
            {
                BloggerId = request.UserId,
                Skip = (request.PageParm.Page - 1)*(request.PageParm.Size),
                Take = request.PageParm.Size
            };
            var list = await _dapperHelper.QueryAsync<QueryArticlePageResponse>(sql, param, CommandFlags.None,
                cancellationToken);
            //查询分类和文章数据缓存
            if (list == null) return null;
            foreach (var a in list)
            {
                //查询分类
                var categorySql = @"select c.Id as CategoryId,c.CategoryName 
                                    from category_association as ca
                                    join category as c on ca.CategoryId = c.Id
                                    where ca.ArticleId=@ArticleId";
                var cParam = new
                {
                    ArticleId=Convert.ToInt64(a.Id)
                };
                var cl = await _dapperHelper.QueryAsync<QueryCategoryResponse>(categorySql, cParam, CommandFlags.None,
                    cancellationToken);
                
                //查询文章数据缓存
                var articleData = await _articleDataRepository.GetByIdAsync(Convert.ToInt64(a.Id));
                a.Like = articleData.Like;
                a.Comment = articleData.Comment;
                a.View = articleData.View;
            }
            //计算分页总数
            var countSql = "select count(*) from article where BloggerId=@BloggerId limit @Skip,@Take";
            var count = await _dapperHelper.QueryFirstOrDefaultAsync<int>(countSql, param, CommandFlags.None,
                cancellationToken);
            return new PageList<QueryArticlePageResponse>(request.PageParm.Page, request.PageParm.Size, count, list);
        }

        /// <summary>
        /// 查询文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<QueryArticleDetailResponse> QueryArticleDetailAsync(QueryArticleDetailRequest request, CancellationToken cancellationToken = default)
        {
            var sql = "select * from article where Id=@ArticleId";
            var article = await _dapperHelper.QueryFirstOrDefaultAsync<QueryArticleDetailResponse>(sql,
                new {request.ArticleId}, CommandFlags.None, cancellationToken);

            //查询文章数据缓存
            var articleData = await _articleDataRepository.GetByIdAsync(Convert.ToInt64(article.Id));
            article.Like = articleData.Like;
            article.Comment = articleData.Comment;
            article.View = articleData.View;
            return article;
        }
    }
}