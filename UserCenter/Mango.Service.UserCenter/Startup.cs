using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Core.Authentication.Extension;
using Mango.Core.Extension;
using Mango.EntityFramework.Extension;
using Mango.Service.UserCenter.Abstraction.Repositories;
using Mango.Service.UserCenter.Abstraction.Services;
using Mango.Service.UserCenter.Repositories;
using Mango.Service.UserCenter.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            services.AddAutoMapper();

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

            #region 仓储注入
            //对于mysql和redis连接以后考虑放在配置中心
            services.AddMangoDbContext<UserCenterDbContext, UserCenterOfWork>(Configuration["MysqlConnection"]);
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserExternalLoginRepository, UserExternalLoginRepository>();
            services.AddScoped<IUserPasswordRepository, UserPasswordRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            #endregion

            #region 服务注入
            services.AddScoped<IUserService, UserService>();
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
