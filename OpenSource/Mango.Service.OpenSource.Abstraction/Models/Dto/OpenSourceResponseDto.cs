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

namespace Mango.Service.OpenSource.Abstraction.Models.Dto
{
    class OpenSourceResponseDto
    {
    }

    //后台
    #region 查询开源仓库响应
    public class QueryOpenSourceProjectAdminResponse
    {
        /// <summary>
        /// 开源id
        /// </summary>
        public long Id { get; set; }

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
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
    #endregion

    //web端请求
    #region 查询开源仓库响应
    public class QueryOpenSourceProjectResponse
    {
        /// <summary>
        /// 开源id
        /// </summary>
        public long Id { get; set; }

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
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
    #endregion
}
