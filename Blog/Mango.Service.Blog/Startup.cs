using Mango.Core.ApiResponse;
using Mango.Core.Authentication.Extension;
using Mango.Core.Cache.Extension;
using Mango.Core.Converter;
using Mango.Core.Extension;
using Mango.Core.Serialization.Extension;
using Mango.EntityFramework.Extension;
using Mango.Infrastructure.Helper;
using Mango.Service.Blog.Abstractions.Repositories;
using Mango.Service.Blog.Abstractions.Services;
using Mango.Service.Blog.Job;
using Mango.Service.Blog.Repositories;
using Mango.Service.Blog.Services;
using Mango.Service.ConfigCenter.Abstraction.Models.Dto;
using Mango.Service.ConfigCenter.Abstraction.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Mango.Service.Blog
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
                ModuleName = "Mango.Blog",
                ModuleSecret = "mango.blog"
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

            #region 授权配置

            var policyKeyPair = new Dictionary<string, string>
            {
                {"client",ModuleConfig.ValidAudience },
                {"admin","mango.admin" }
            };

            services.AddMangoJwtPolicy(policyKeyPair, opt =>
            {
                opt.Key = GlobalConfig.JWTKey;
                opt.Issuer = ModuleConfig.ValidIssuer;
            });

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

            #region 基础仓储，服务配置

            services.AddAutoMapper();
            services.AddMangoRedis(op =>
            {
                op.ConnectionString = ModuleConfig.RedisConnectionString;
            });

            services.AddMangoDbContext<BlogDbContext, BlogOfWork>(ModuleConfig.MysqlConnectionString);

            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleDetailRepository, ArticleDetailRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            services.AddScoped<IArticleCacheService, ArticleCacheService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IJobService, JobService>();

            //services.AddHostedService<ArticleJobService>();

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
