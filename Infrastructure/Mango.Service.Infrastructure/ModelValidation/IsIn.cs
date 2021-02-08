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

using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mango.Service.Infrastructure.ModelValidation
{
    /// <summary>
    /// 离散值验证
    /// </summary>
    public class IsIn:ValidationAttribute
    {
        private readonly object[] _o;
        
        public IsIn(params object[] o)
        {
            _o = o;
        }

        /// <summary>
        /// 重载判断
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var flag = _o.Any(i => i == value);
            return flag ? true : false;
        }
    }
}