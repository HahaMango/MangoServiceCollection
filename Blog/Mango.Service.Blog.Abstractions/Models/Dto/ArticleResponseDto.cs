using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Abstractions.Models.Dto
{
    public class ArticleResponseDto
    {
    }

    #region 文章分页列表响应类

    /// <summary>
    /// 文章分页相应类
    /// </summary>
    public class ArticlePageListResponse
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 文章分类Id
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// 阅读数
        /// </summary>
        public int View { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int Comment { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int Like { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public int IsTop { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    #endregion

    #region 文章详情响应类

    public class ArticleDetailResponse
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 作者Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 作者昵称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 文章格式类型 0:MD
        /// </summary>
        public int ContentType { get; set; }

        /// <summary>
        /// 文章分类Id
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// 阅读数
        /// </summary>
        public int View { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int Comment { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int Like { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    #endregion
}
