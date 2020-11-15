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

using Mango.Service.Infrastructure.Persistence;
using Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate.Enum;
using System;

namespace Mango.Service.OpenSource.Domain.AggregateModel.ProjectAggregate
{
    /// <summary>
    /// 开源项目聚合根
    /// </summary>
    public class Project : AggregateRoot
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

        /// <summary>
        /// 排序Id
        /// </summary>
        private int _sortId { get; set; }

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
        public Project(long userId, string projectName, string desc, string repositoryUrl, string image, string readme, string platform, int sortId = 0)
        {
            _userId = userId;
            ProjectInfo = new ProjectInfo(projectName, desc, repositoryUrl, image, readme, platform);
            _sortId = sortId;
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
        }

        /// <summary>
        /// 启用
        /// </summary>
        public void Enable()
        {
            Status = ProjectStatusEnum.Open;
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
        public void EditProjectInfo(string projectName, string desc, string repositoryUrl, string image, string readme, string platform, int sortId = 0)
        {
            ProjectInfo = new ProjectInfo(projectName, desc, repositoryUrl, image, readme, platform);
            _sortId = sortId;
        }
    }
}
