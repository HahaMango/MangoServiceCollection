﻿/*--------------------------------------------------------------------------
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

using Mango.EntityFramework.Repositories;
using Mango.Service.ConfigCenter.Abstraction.Models.Entities;
using Mango.Service.ConfigCenter.Abstraction.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.ConfigCenter.Repositories
{
    /// <summary>
    /// ip白名单仓储接口实现
    /// </summary>
    public class IPWhiteListRepository : EfRepository<ConfigCenterDbContext, IPWhiteList>, IIPWhiteListRepository
    {
        public IPWhiteListRepository(ConfigCenterDbContext configCenterDbContext) : base(configCenterDbContext)
        {

        }
    }
}
