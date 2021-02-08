/*--------------------------------------------------------------------------
//
//  Copyright 2021 Chiva Chen
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

namespace Mango.Service.Blog.Infrastructure.Config
{
    /// <summary>
    /// 缓存key配置
    /// </summary>
    public static class RedisConfig
    {
        /// <summary>
        /// 文章点赞缓存
        /// </summary>
        public const string Article_Like_key = "article:like";

        /// <summary>
        /// 文章评论缓存
        /// </summary>
        public const string Article_Comment_Key = "article:comment";

        /// <summary>
        /// 文章阅读数缓存
        /// </summary>
        public const string Article_View_Key = "article:view";
    }
}