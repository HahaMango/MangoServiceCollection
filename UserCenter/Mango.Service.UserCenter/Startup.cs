using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Core.ApiResponse;
using Mango.Core.Authentication.Extension;
using Mango.Core.Config;
using Mango.Core.Converter;
using Mango.Core.DataStructure;
using Mango.Core.Extension;
using Mango.Core.HttpService;
using Mango.Core.Serialization.Extension;
using Mango.Core.Srd;
using Mango.Core.Srd.Extension;
using Mango.EntityFramework.Extension;
using Mango.Infrastructure.Helper;
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
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.Converters.Add(new IntConverter());
                    o.JsonSerializerOptions.Converters.Add(new NullableIntConverter());
                    o.JsonSerializerOptions.Converters.Add(new LongConverter());
                    o.JsonSerializerOptions.Converters.Add(new NullableLongConverter());
                    o.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    o.JsonSerializerOptions.Converters.Add(new NullableDateTimeConverter());
                });

            #region 配置中心
            var token = Configuration["Consul:Token"];
            var configKey = Configuration["Service:ConfigKey"];
            var consulIp = Configuration["Consul:Ip"];
            var port = Configuration["Consul:Port"];

            var config = new MangoConfig($"http://{consulIp}:{port}", token);

            var moduleConfig = config.GetConfig(configKey).Result;
            var globalConfig = config.GetConfig("mango/global").Result;
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

            #region jwt配置

            services.AddMangoJwtHandler(options =>
            {
                options.Key = globalConfig.JwtKey;
                options.DefalutAudience = moduleConfig.ValidAudience;
                options.DefalutIssuer = moduleConfig.ValidIssuer;
            });

            var policyKeyPair = new Dictionary<string, string>
            {
                {"client",moduleConfig.ValidAudience },
                {"admin","mango.admin" }
            };

            services.AddMangoJwtPolicy(policyKeyPair, opt =>
            {
                opt.Key = moduleConfig.JwtKey;
                opt.Issuer = moduleConfig.ValidIssuer;
            });

            #endregion

            #region 仓储注入
            //对于mysql和redis连接以后考虑放在配置中心
            services.AddMangoDbContext<UserCenterDbContext, UserCenterOfWork>(moduleConfig.DbConnectString);
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserExternalLoginRepository, UserExternalLoginRepository>();
            services.AddScoped<IUserPasswordRepository, UserPasswordRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUserAboutRepository, UserAbountRepository>();
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
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

            #region 服务注册
            if (env.IsProduction())
            {
                var consulIp = Configuration["Consul:Ip"];
                var port = Configuration["Consul:Port"];
                var serviceName = Configuration["Service:Name"];
                var servicePort = Configuration["Service:Port"];
                var healthCheck = Configuration["Service:HealthCheck"];
                var currentIp = NetworkHelper.FirstInternalLocalAddress().ToString();

                var rc = new ConsulRegistration($"http://{consulIp}:{port}")
                {
                    HealthCheckUrl = $"http://{currentIp}:{servicePort}/{healthCheck}"
                };

                var se = new MangoService
                {
                    Id = Guid.NewGuid().ToString(),
                    IP = currentIp,
                    Port = servicePort,
                    ServiceName = serviceName
                };
                app.RegisterConsulService(rc, se, lifetime);
            }
            #endregion

        }
    }
}
