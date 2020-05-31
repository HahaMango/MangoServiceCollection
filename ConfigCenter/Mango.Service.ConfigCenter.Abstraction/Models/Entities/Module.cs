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

using Mango.Core.EntityFramework.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mango.Service.ConfigCenter.Abstraction.Models.Entities
{
    [Table("module")]
    public class Module : SnowFlakeEntity
    {
        /// <summary>
        /// 模块名称(全局唯一)
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块密码
        /// </summary>
        public string ModuleSecret { get; set; }

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
        /// 最后加载配置时间
        /// </summary>
        public DateTime? LastLoadTime { get; set; }

        /// <summary>
        /// 最后一次加载配置的Ip地址
        /// </summary>
        public string LastLoadIp { get; set; }

        public Module() { }

        public Module(bool set)
        {
            base.SetId();
        }
    }
}
