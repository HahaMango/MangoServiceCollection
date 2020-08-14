using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mango.Core.ApiResponse;
using Mango.Core.Extension;
using Mango.Core.Serialization.Extension;
using Mango.EntityFramework.Extension;
using Mango.Service.ConfigCenter.Abstraction.Repositories;
using Mango.Service.ConfigCenter.Abstraction.Services;
using Mango.Service.ConfigCenter.Repositories;
using Mango.Service.ConfigCenter.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mango.Service.ConfigCenter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper();

            #region 添加数据库
            services.AddMangoDbContext<ConfigCenterDbContext, ConfigCenterOfWork>(Configuration["MysqlConnection"]);
            #endregion

            #region 添加仓储
            services.AddScoped<IGlobalConfigRepository, GlobalConfigRepository>();
            services.AddScoped<IModuleConfigRepository, ModuleConfigRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IIPWhiteListRepository, IPWhiteListRepository>();
            #endregion

            #region 添加服务
            services.AddScoped<IGlobalConfigService, GlobalConfigService>();
            services.AddScoped<IModuleConfigService, ModuleConfigService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IIPWhiteListService, IPWhiteListService>();
            #endregion

            #region 其他组件注入
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            #region 跨域配置
            services.AddCors(config =>
            {
                config.AddPolicy("all", p =>
                {
                    p.SetIsOriginAllowed(op => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("all");
            app.UseAuthorization();

            app.Use(async (httpContext, next) =>
            {
                var response = new ApiResult();
                var ipService = httpContext.RequestServices.GetService<IIPWhiteListService>();
                var currentIP = httpContext.Connection.RemoteIpAddress.ToString();
                currentIP = "193.134.12.1";
                var list = await ipService.IsMatchAsync(currentIP);
                if (list.Code != Core.Enums.Code.Ok)
                {
                    httpContext.Response.StatusCode = 401;
                    httpContext.Response.ContentType = "application/json; charset=utf-8";
                    response.Code = Core.Enums.Code.Unauthorized;
                    response.Message = $"{currentIP} 该IP无访问权限";
                    Console.WriteLine("日志："+response.ToJsonUtf8());
                    await httpContext.Response.WriteAsync(response.ToJsonUtf8());
                }
                if (!httpContext.Response.HasStarted)
                {
                    await next.Invoke();
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
