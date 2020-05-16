using Mango.Core.DataStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Abstractions.Models.Dto
{
    public class ArticleRequestDto
    {

    }

    #region 添加文章请求类

    public class AddArticleRequest
    {
        /// <summary>
        /// 用户Id （目前只个人使用不需要传参）
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 文章类目Id (为空使用默认分类)
        /// </summary>
        public long? CategoryId { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 文章内容格式类型 0：MD 目前仅支持MD
        /// </summary>
        public int ContentType { get; set; } 
    }

    #endregion

    #region 文章分页请求类

    public class ArticlePageRequest
    {
        /// <summary>
        /// 分页参数
        /// </summary>
        public PageParm PageParm { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 作者名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 筛选开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 筛选结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    #endregion

    #region 文章详情请求类

    public class ArticleDetailRequest
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public long ArticleId { get; set; }
    }

    #endregion

    #region 文章点赞请求类

    public class ArticleLikeRequest
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public long ArticleId { get; set; }
    }

    #endregion

    #region 文章评论请求类

    public class CommentRequest
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public long ArticleId { get; set; }

        /// <summary>
        /// 评论显示用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
    }

    #endregion
}
