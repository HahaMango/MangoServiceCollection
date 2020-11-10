using Mango.Core.ApiResponse;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Mango.Service.Infrastructure.Behaviors
{
    public class TryCatchBehaviors<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : ApiResult
    {
        private readonly ILogger<TryCatchBehaviors<TRequest, TResponse>> _logger;

        public TryCatchBehaviors(ILogger<TryCatchBehaviors<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch(Exception ex)
            {
                _logger.LogError($"TryCatchBehaviors异常...{ex.Message}");
                var responseType = typeof(TResponse);
                var c = responseType.GetConstructor(Type.EmptyTypes);
                var response = (TResponse)c.Invoke(null);
                response.Code = Core.Enums.Code.Error;
                response.Message = $"TryCatchBehaviors异常...Request:{request.GetType().Name} | Response:{response.GetType().Name}";
                return response;
            }
        }
    }
}
