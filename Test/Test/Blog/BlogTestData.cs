using Mango.Core.Serialization.Extension;
using Mango.Service.Blog.Abstractions.Models.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Test.Blog
{
    public class BlogTestData
    {
        #region 添加文章
        public static IEnumerable<object[]> AddArticleData
        {
            get
            {
                using (var dataFile = new StreamReader("Data/Blog/AddArticle.json"))
                {
                    var datajson = dataFile.ReadToEnd();
                    var datas = datajson.ToObjectAsync<List<AddArticleJson>>().Result;
                    var result = new List<object[]>(datas.Count);
                    foreach(var data in datas)
                    {
                        result.Add(new object[] { data.Request, data.UserId, data.Code });
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
            /// 用户ID
            /// </summary>
            public string UserId { get; set; }

            /// <summary>
            /// 响应码
            /// </summary>
            public int Code { get; set; }
        }

        #endregion
    }
}
