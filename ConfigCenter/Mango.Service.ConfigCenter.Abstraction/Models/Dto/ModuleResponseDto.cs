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

using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.ConfigCenter.Abstraction.Models.Dto
{
    public class ModuleResponseDto
    {
    }

    #region 查询模块配置
    public class ModuleConfigResponse
    {
        /// <summary>
        /// 模块Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 模块名称(全局唯一)
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块密码
        /// </summary>
        public string ModuleSecret { get; set; }

        /// <summary>
        /// mysql连接字符串
        /// </summary>
        public string MysqlConnectionString { get; set; }

        /// <summary>
        /// redis连接字符串
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// JWT密钥
        /// </summary>
        public string JWTKey { get; set; }

        /// <summary>
        /// JWT有效的域
        /// </summary>
        public string ValidAudience { get; set; }

        /// <summary>
        /// JWT有效的颁发机构
        /// </summary>
        public string ValidIssuer { get; set; }

        /// <summary>
        /// 其他配置（Json格式储存）
        /// </summary>
        public string OtherConfig { get; set; }
    }
    #endregion
}
