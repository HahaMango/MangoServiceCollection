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

using Mango.Core.Converter;
using Mango.Core.Serialization.Extension;
using Mango.Service.Blog.Abstractions.Models.Dto;
using Mango.Service.Blog.Abstractions.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Mango.Service.Blog.Test
{
    public class BlogTestData
    {
        #region 添加文章
        public static IEnumerable<object[]> AddArticleData
        {
            get
            {
                using (var dataFile = new StreamReader("Data/AddArticle.json"))
                {
                    var datajson = dataFile.ReadToEnd();
                    var datas = datajson.ToObjectAsync<List<AddArticleJson>>().Result;
                    var result = new List<object[]>(datas.Count);
                    foreach (var data in datas)
                    {
                        result.Add(new object[] { data.Request,data.Category, data.UserId, data.Code });
                    }
                    return result;
                }
            }
        }

        class AddArticleJson
        {
            /// <summary>
            /// 添加文章请求
            /// </summary>
            public AddArticleRequest Request { get; set; }

            /// <summary>
            /// 分类
            /// </summary>
            public Category Category { get; set; }

            /// <summary>
            /// 用户ID
            /// </summary>
            public string UserId { get; set; }

            /// <summary>
            /// 响应码
            /// </summary>
            public int Code { get; set; }
        }

        #endregion

        #region 点赞文章
        public static IEnumerable<object[]> LikeData
        {
            get
            {
                using (var dataFile = new StreamReader("Data/Like.json"))
                {
                    var datajson = dataFile.ReadToEnd();
                    var datas = datajson.ToObjectAsync<List<LikeJson>>().Result;
                    var result = new List<object[]>(datas.Count);
                    foreach (var data in datas)
                    {
                        result.Add(new object[] { data.Request, data.Code });
                    }
                    return result;
                }
            }
        }

        class LikeJson
        {
            public ArticleLikeRequest Request { get; set; }

            public int Code { get; set; }
        }
        #endregion

        #region 查询文章详情
        public static IEnumerable<object[]> DetailData
        {
            get
            {
                using (var dataFile = new StreamReader("Data/Detail.json"))
                {
                    var datajson = dataFile.ReadToEnd();
                    var datas = datajson.ToObjectAsync<List<DetailJson>>().Result;
                    var result = new List<object[]>(datas.Count);
                    foreach (var data in datas)
                    {
                        result.Add(new object[] { data.Article, data.ArticleDetail, data.ArticleId, data.Code });
                    }
                    return result;
                }
            }
        }

        class DetailJson
        {
            public Article Article { get; set; }

            public ArticleDetail ArticleDetail { get; set; }

            public long ArticleId { get; set; }

            public int Code { get; set; }
        }
        #endregion
    }
}
