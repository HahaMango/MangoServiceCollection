using Mango.EntityFramework.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mango.Service.Blog.Abstractions.Models.Entities
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("user")]
    public class User:SnowFlakeEntity
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

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public User() { }

        /// <summary>
        /// 设置Id
        /// </summary>
        /// <param name="set"></param>
        public User(bool set)
        {
            base.SetId();
        }
    }
}
