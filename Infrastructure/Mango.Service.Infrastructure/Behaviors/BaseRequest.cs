using Mango.Core.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Infrastructure.Behaviors
{
    /// <summary>
    /// Try Catch MediatR请求接口
    /// </summary>
    public interface ITryCatchRequest : IRequest<ApiResult>
    {

    }

    /// <summary>
    /// 事务MediatR请求接口
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public interface ITransactionRequest<TResponse> : IRequest<TResponse>
    {

    }
}
