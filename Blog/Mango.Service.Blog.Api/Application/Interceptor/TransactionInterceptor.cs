using AspectCore.DynamicProxy;
using Mango.Core.ApiResponse;
using Mango.Core.Enums;
using Mango.Service.Blog.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Api.Application.Interceptor
{
    /// <summary>
    /// 事务拦截器
    /// </summary>
    public class TransactionInterceptor : AbstractInterceptorAttribute
    {
        private BlogDbContext _context;

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            _context = (BlogDbContext)context.ServiceProvider.GetService(typeof(BlogDbContext));
            var returnType = context.ImplementationMethod.ReturnType;

            if(_context.DbContextTransaction != null)
            {
                await next(context);
                return;
            }

            try
            {
                await _context.BeginTransactionAsync();

                await next(context);

                dynamic returnValue = context.ReturnValue;
                //异步返回值
                if(returnType != null && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    var genericType = returnType.GetGenericArguments()[0];
                    if(genericType == typeof(ApiResult) || genericType.GetEnumUnderlyingType() == typeof(ApiResult<>))
                    {
                        if(returnValue.Result != null && returnValue.Result.Code != Code.Ok)
                        {
                            await _context.RollbackAsync();
                            return;
                        }
                    }
                }else if (returnType != null)
                {
                    var returnTypeInterface = returnType.GetInterfaces().FirstOrDefault(i => i.FullName == typeof(ApiResult).FullName);
                    if (returnTypeInterface != null || returnType == typeof(ApiResult))
                    {
                        if(returnValue != null && returnValue.Code != Code.Ok)
                        {
                            await _context.RollbackAsync();
                            return;
                        }
                    }
                }

                await _context.CommitAsync();
            }
            catch(Exception ex)
            {
                await _context.RollbackAsync();
                //异步返回值
                if (returnType != null && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    var genericType = returnType.GetGenericArguments()[0];
                    var genericTypeInterface = genericType.GetInterfaces().FirstOrDefault(i => i.FullName == typeof(ApiResult).FullName);
                    if(genericTypeInterface != null || genericType == typeof(ApiResult))
                    {
                        dynamic returnValue = Activator.CreateInstance(genericType);
                        returnValue.Code = Code.Error;
                        returnValue.Message = $"事务拦截器异常，{ex.Message}";
                        context.ReturnValue = Task.FromResult(returnValue);
                        return;
                    }
                }else if(returnType != null)
                {
                    var returnTypeInterface = returnType.GetInterfaces().FirstOrDefault(i => i.FullName == typeof(ApiResult).FullName);
                    if(returnTypeInterface != null || returnType == typeof(ApiResult))
                    {
                        dynamic returnValue = Activator.CreateInstance(returnType);
                        returnValue.Code = Code.Error;
                        returnValue.Message = $"事务拦截器异常，{ex.Message}";
                        context.ReturnValue = returnValue;
                        return;
                    }
                }
            }
        }
    }
}
