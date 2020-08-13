using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Core.ApiResponse;
using Mango.Core.Authentication.Extension;
using Mango.Core.Extension;
using Mango.Core.HttpService;
using Mango.Core.Serialization.Extension;
using Mango.EntityFramework.Extension;
using Mango.Infrastructure.Helper;
using Mango.Infrastructure.HttpService;
using Mango.Service.ConfigCenter.Abstraction.Models.Dto;
using Mango.Service.ConfigCenter.Abstraction.Models.Entities;
using Mango.Service.UserCenter.Abstraction.Models.Entities;
using Mango.Service.UserCenter.Abstraction.Repositories;
using Mango.Service.UserCenter.Abstraction.Services;
using Mango.Service.UserCenter.Repositories;
using Mango.Service.UserCenter.Services;
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

namespace Mango.Service.UserCenter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var globalConfigRespose = HttpHelper.GetAsync<ApiResult<GlobalConfig>>("http://configcenter.hahamango.cn/api/configcenter/global").Result;
            if (!globalConfigRespose.IsSuccessStatusCode)
            {
                throw new Exception("访问配置中心异常");
            }
            if (globalConfigRespose.Data.Code != Core.Enums.Code.Ok)
            {
                throw new Exception($"查询全局配置异常,{globalConfigRespose.Data.Message}");
            }
            GlobalConfig = globalConfigRespose.Data.Data;

            var configRequest = new QueryModuleConfigRequest
            {
                ModuleName = "Mango.UserCenter",
                ModuleSecret = "mango.usercenter"
            };
            var moduleConfigResponse = HttpHelper.PostAsync<ApiResult<ModuleConfigResponse>>("http://configcenter.hahamango.cn/api/configcenter/queryconfig", configRequest.ToJson()).Result;
            if (!moduleConfigResponse.IsSuccessStatusCode)
            {
                throw new Exception("访问配置中心异常");
            }
            if (moduleConfigResponse.Data.Code != Core.Enums.Code.Ok)
            {
                throw new Exception($"查询模块配置异常，{moduleConfigResponse.Data.Message}");
            }
            ModuleConfig = moduleConfigResponse.Data.Data;
        }

        public IConfiguration Configuration { get; }

        public GlobalConfig GlobalConfig { get; }

        public ModuleConfigResponse ModuleConfig { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

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

            #region jwt配置

            services.AddMangoJwtHandler(options =>
            {
                options.Key = GlobalConfig.JWTKey;
                options.DefalutAudience = ModuleConfig.ValidAudience;
                options.DefalutIssuer = ModuleConfig.ValidIssuer;
            });

            services.AddMangoJwtAuthentication(options =>
            {
                options.Key = GlobalConfig.JWTKey;
                options.Audience = ModuleConfig.ValidAudience;
                options.Issuer = ModuleConfig.ValidIssuer;
            });

            #endregion

            #region 仓储注入
            //对于mysql和redis连接以后考虑放在配置中心
            services.AddMangoDbContext<UserCenterDbContext, UserCenterOfWork>(ModuleConfig.MysqlConnectionString);
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserExternalLoginRepository, UserExternalLoginRepository>();
            services.AddScoped<IUserPasswordRepository, UserPasswordRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            #endregion

            #region 服务注入
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserPasswordService, UserPasswordService>();
            #endregion

            #region 其他组件注入
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper();
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
