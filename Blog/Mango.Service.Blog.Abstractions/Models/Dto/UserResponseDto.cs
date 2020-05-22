using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Blog.Abstractions.Models.Dto
{
    public class UserResponseDto
    {
    }

    #region 用户信息响应类

    public class UserInfoResponse
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 头像图片URL
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// github链接
        /// </summary>
        public string GitHub { get; set; }

        /// <summary>
        /// 个人主页
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// IG链接
        /// </summary>
        public string IG { get; set; }

        /// <summary>
        /// 技能介绍
        /// </summary>
        public string Skill { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }
    }

    #endregion
}
