﻿using Mango.Core.ApiResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Infrastructure.Helper
{
    /// <summary>
    /// ApiResult 返回包装帮助类
    /// </summary>
    public abstract class ApiResultHepler
    {
        public virtual ApiResult NotFound(string message = "找不到资源")
        {
            return new ApiResult
            {
                Code = Core.Enums.Code.NoFound,
                Message = message
            };
        }

        public virtual ApiResult<T> NotFound<T>(string message = "找不到资源")
        {
            return new ApiResult<T>
            {
                Code = Core.Enums.Code.NoFound,
                Message = message
            };
        }

        public virtual ApiResult Ok(string message = "成功")
        {
            return new ApiResult
            {
                Code = Core.Enums.Code.Ok,
                Message = message
            };
        }

        public virtual ApiResult<T> Ok<T>(string message = "成功")
        {
            return new ApiResult<T>
            {
                Code = Core.Enums.Code.Ok,
                Message = message
            };
        }

        public virtual ApiResult Fail(string message = "操作失败")
        {
            return new ApiResult
            {
                Code = Core.Enums.Code.Error,
                Message = message
            };
        }

        public virtual ApiResult<T> Fail<T>(string messsage = "操作失败")
        {
            return new ApiResult<T>
            {
                Code = Core.Enums.Code.Error,
                Message = messsage
            };
        }
    }
}
