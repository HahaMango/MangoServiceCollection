using Mango.EntityFramework.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mango.Service.Blog.Abstractions.Models.Entities
{
    /// <summary>
    /// 文章分类表
    /// </summary>
    [Table("category")]
    public class Category : SnowFlakeEntity
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 状态 0：删除 1：正常
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否默认分类
        /// </summary>
        public int IsDefault { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Category() { }

        /// <summary>
        /// 设置Id
        /// </summary>
        /// <param name="set"></param>
        public Category(bool set)
        {
            base.SetId();
        }
    }
}
