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
    /// <summary>
    /// 访问Ip白名单
    /// </summary>
    [Table("ip_white_list")]
    public class IPWhiteList : SnowFlakeEntity
    {
        /// <summary>
        /// ip地址
        /// </summary>
        public string IP { get; set; }

        public IPWhiteList() { }

        /// <summary>
        /// 设置Id
        /// </summary>
        /// <param name="set"></param>
        public IPWhiteList(bool set)
        {
            base.SetId();
        }
    }
}
