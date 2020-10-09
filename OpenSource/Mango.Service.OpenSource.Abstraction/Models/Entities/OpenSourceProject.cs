using Mango.Core.EntityFramework.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.OpenSource.Abstraction.Models.Entities
{
    public class OpenSourceProject : SnowFlakeEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 开源仓库地址
        /// </summary>
        public string RepositoryUrl { get; set; }

        /// <summary>
        /// 开源项目图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 开源项目README文本
        /// </summary>
        public string README { get; set; }

        /// <summary>
        /// 开源平台
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// 状态 0：删除 1：正常
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public OpenSourceProject() { }

        /// <summary>
        /// 初始化ID
        /// </summary>
        /// <param name="set"></param>
        public OpenSourceProject(bool set)
        {
            base.SetId();
        }
    }
}
