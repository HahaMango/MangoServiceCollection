using Mango.Core.ApiResponse;
using Mango.Core.Authentication.Extension;
using Mango.Core.Cache.Extension;
using Mango.Core.Converter;
using Mango.Core.Extension;
using Mango.EntityFramework.Extension;
using Mango.Infrastructure.Helper;
using Mango.Service.Blog.Abstractions.Repositories;
using Mango.Service.Blog.Abstractions.Services;
using Mango.Service.Blog.Job;
using Mango.Service.Blog.Repositories;
using Mango.Service.Blog.Services;
using Mango.Service.ConfigCenter.Abstraction.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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
                throw new Exception("·ÃÎÊÅäÖÃÖÐÐÄÒì³£");
            }
            if (globalConfigRespose.Data.Code != Core.Enums.Code.Ok)
            {
                throw new Exception($"²éÑ¯È«¾ÖÅäÖÃÒì³£,{globalConfigRespose.Data.Message}");
            }
            GlobalConfig = globalConfigRespose.Data.Data;
        }

        public IConfiguration Configuration { get; }

        public GlobalConfig GlobalConfig { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(o=>
                {
                    o.JsonSerializerOptions.Converters.Add(new IntConverter());
                    o.JsonSerializerOptions.Converters.Add(new NullableIntConverter());
                    o.JsonSerializerOptions.Converters.Add(new LongConverter());
                    o.JsonSerializerOptions.Converters.Add(new NullableLongConverter());
                    o.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    o.JsonSerializerOptions.Converters.Add(new NullableDateTimeConverter());
                });

            #region ÊÚÈ¨ÅäÖÃ

            services.AddMangoJwtAuthentication(options =>
            {
                options.Key = GlobalConfig.JWTKey;
                options.Audience = "mango.blog";
                options.Issuer = "hahamango.cn";
            });

            #endregion

            #region ¿çÓòÅäÖÃ

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

            #region »ù´¡²Ö´¢£¬·þÎñÅäÖÃ

            services.AddAutoMapper();
            services.AddMangoRedis(op =>
            {
                op.ConnectionString = Configuration["RedisConnection"];
            });

            services.AddMangoDbContext<BlogDbContext, BlogOfWork>(Configuration["MysqlConnection"]);

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
