/*--------------------------------------------------------------------------*/
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

namespace Mango.Service.Blog.Abstractions.CacheConfig
{
    /// <summary>
    /// 文章缓存配置类
    /// </summary>
    public static class ArticleCacheConfig
    {
        /// <summary>
        /// 阅读数键值
        /// </summary>
        public static string VIEW_CACHE_KEY = "viewcachekey:";

        /// <summary>
        /// 点赞数键值
        /// </summary>
        public static string LIKE_CACHE_KEY = "likecachekey:";

        /// <summary>
        /// 阅读数持久化阈值
        /// </summary>
        public static int LIMIT_VIEW = 30;

        /// <summary>
        /// 点赞数持久化阈值
        /// </summary>
        public static int LIMIT_LIKE = 5;
    }
}
