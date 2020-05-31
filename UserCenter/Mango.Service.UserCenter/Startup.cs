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
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //var globalConfigHttp = new JsonHttpService<ApiResult<GlobalConfig>>(new System.Net.Http.HttpClient());
            //var moduleConfigHttp = new JsonHttpService<ApiResult<ModuleConfigResponse>>(new System.Net.Http.HttpClient());
            ////ȫ������
            //var globalConfigResponse = globalConfigHttp.GetAsync("https://localhost:5001/api/configcenter/global").Result;
            ////ģ������
            //var queryConfig = new QueryModuleConfigRequest
            //{
            //    ModuleName = "Mango.Blog",
            //    ModuleSecret = "mango.blog"
            //};
            //var moduleConfigResponse = moduleConfigHttp.PostAsync("https://localhost:5001/api/configcenter/queryconfig", queryConfig.ToJson()).Result;

            services.AddAutoMapper();

            #region ��������

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

            #region jwt����

            services.AddMangoJwtHandler(options =>
            {
                options.Key = Configuration["Jwt:Key"];
                options.DefalutAudience = Configuration["Jwt:Audience"];
                options.DefalutIssuer = Configuration["Jwt:Issuer"];
            });

            services.AddMangoJwtAuthentication(options =>
            {
                options.Key = Configuration["Jwt:Key"];
                options.Audience = Configuration["Jwt:Audience"];
                options.Issuer = Configuration["Jwt:Issuer"];
            });

            #endregion

            #region �ִ�ע��
            //����mysql��redis�����Ժ��Ƿ�����������
            services.AddMangoDbContext<UserCenterDbContext, UserCenterOfWork>(Configuration["MysqlConnection"]);
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserExternalLoginRepository, UserExternalLoginRepository>();
            services.AddScoped<IUserPasswordRepository, UserPasswordRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            #endregion

            #region ����ע��
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserPasswordService, UserPasswordService>();
            #endregion

            #region �������ע��
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("all");
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
