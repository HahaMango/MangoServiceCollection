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

using Mango.EntityFramework.Abstractions;
using Mango.EntityFramework.BaseEntity;
using Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate
{
    /// <summary>
    /// 开源项目聚合根
    /// </summary>
    public class Project : SnowFlakeEntity, IAggregateRoot
    {
        /// <summary>
        /// 项目信息
        /// </summary>
        public ProjectInfo ProjectInfo { get; private set; }

        /// <summary>
        /// 开源项目状态
        /// </summary>
        public ProjectStatusEnum Status { get; private set; }

        /// <summary>
        /// 开源项目拥有者
        /// </summary>
        private long _userId;

        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime _createTime;

        /// <summary>
        /// 更新时间
        /// </summary>
        private DateTime? _updateTime;

        public Project()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectName"></param>
        /// <param name="desc"></param>
        /// <param name="repositoryUrl"></param>
        /// <param name="image"></param>
        /// <param name="readme"></param>
        /// <param name="platform"></param>
        public Project(long userId, string projectName, string desc, string repositoryUrl, string image, string readme, string platform)
        {
            _userId = userId;
            ProjectInfo = new ProjectInfo(projectName, desc, repositoryUrl, image, readme, platform);
            _createTime = DateTime.Now;
            Enable();
            SetId();
        }

        /// <summary>
        /// 禁用
        /// </summary>
        public void Disable()
        {
            Status = ProjectStatusEnum.Close;
            RefreshUpdateTime();
        }

        /// <summary>
        /// 启用
        /// </summary>
        public void Enable()
        {
            Status = ProjectStatusEnum.Open;
            RefreshUpdateTime();
        }

        /// <summary>
        /// 编辑项目信息
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="desc"></param>
        /// <param name="repositoryUrl"></param>
        /// <param name="image"></param>
        /// <param name="readme"></param>
        /// <param name="platform"></param>
        public void EditProjectInfo(string projectName, string desc, string repositoryUrl, string image, string readme, string platform)
        {
            ProjectInfo = new ProjectInfo(projectName, desc, repositoryUrl, image, readme, platform);
            RefreshUpdateTime();
        }

        /// <summary>
        /// 刷新更新时间
        /// </summary>
        private void RefreshUpdateTime()
        {
            //_updateTime = DateTime.Now;
        }
    }
}
