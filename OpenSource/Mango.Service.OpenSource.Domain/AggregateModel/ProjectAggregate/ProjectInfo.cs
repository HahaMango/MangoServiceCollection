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

namespace Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate
{
    /// <summary>
    /// 项目信息值对象
    /// </summary>
    public class ProjectInfo
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; private set; }

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

        public ProjectInfo() { }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="desc"></param>
        /// <param name="repositoryUrl"></param>
        /// <param name="image"></param>
        /// <param name="readme"></param>
        /// <param name="platform"></param>
        public ProjectInfo(string projectName, string desc, string repositoryUrl, string image, string readme, string platform)
        {
            this.ProjectName = projectName;
            this.Desc = desc;
            this.RepositoryUrl = repositoryUrl;
            this.Image = image;
            this.README = readme;
            this.Platform = platform;
        }
    }
}
