using Mango.Core.ApiResponse;
using Mango.Core.Authentication.Extension;
using Mango.Core.Cache.Extension;
using Mango.Core.Config;
using Mango.Core.Converter;
using Mango.Core.DataStructure;
using Mango.Core.Extension;
using Mango.Core.Serialization.Extension;
using Mango.Core.Srd;
using Mango.Core.Srd.Extension;
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
        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Env { get; }

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
            var moduleConfig = default(MangoServiceConfiguration);
            var globalConfig = default(MangoServiceConfiguration);
            if (Env.IsDevelopment())
            {
                //测试环境
                globalConfig = new MangoServiceConfiguration
                {
                    JwtKey = @"MIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQDJQJY/vIlQV8Hm
y41w6zSJRL7zo/57uj0T6OrJXgzo9pk5mtCWKW/+KTmQ7OCZVaIXKq+5OW1ibkjI
QDJFRBCbUoUN8799GiadCQJDzpcaGh5ev3wjxzqG8yR0bkkgMNkoYhX7IwJ+6B+1
zYsyQeVvhztyO2Rj79lId7oSO4Ku2qDomNe1LaBQP6rESKwdx6TIWPUr4CdbnLGX
AmJ95P66RQ8mqfeHbqvlbZhK/eUF0kNNxbCV0qH4lTFSugSMQaKRNg0H1tlc86dp
lyZpvf8KEekmNYwY1TZSTC4HwVzDiUn6+yh5EbkGlcC7tO1ClntGHwBGjukLKBk5
sjx5BgT/AgMBAAECggEBAK7qvOw+sNYswDIZnyCSvYHFR3Y5hhkBwM5KNa65WN0u
X/TKUxsAfv9X01ncGEYNQKmEB2Ekwaa4lfe+nDLJuulU6qI6xac1EHSSfO50Y65j
HxxYr8vlAECEmZ28sUASVNwdjF9PiX7Fv7HjKWWQEptB3XAmoNWfhKnQrd/k62uO
u4+n3Uej/cH+l1RpQQDqy15ojSIBwBDpZFVPJOt/oQpBgF0V/3NxJSiwAnJCWau7
vGeqFQPlNKW0AldE0Kv3l67FTXC97nnpWdihIvBM0sc8FLNGGg6B4GLTct+Yf5R0
mPI2KHL3o7TSburKcA03AQ+c2EQ3yh9edgRDa7G4pIECgYEA+yjIWIRU20TGDx/R
5ox0bdlZyeSQggwuS8rY2k6sU2hFa513WFB5R6duv47QVKEwGKlnccGrtvU2vNQF
fHYyB9odpkMCQphSUEKy2UrWEB4LJmekUFP4+AjXO0JOEsrQ88hXeSnpC2UXuZe5
fWn+udeUYrEFondCilBomJzA6r8CgYEAzSGQSXokpAec5Cvm0mTGIM1/H7nG7Jm2
BbJ5oyyieebGi35GY9RYrCpOIUkP01cBQaRhIv7ftlu6f4lEH/VpxCIRnA7AROsl
rkdQVAkWU99szd2/6zcAoAs6vU1n5SWF4cF6E/xMCney+x8tGYMWozRwha27uip+
YqJ4DY1ktcECgYEAmuE1WtCP+39Xm7AFomRuz+a+peZfUejnkvxnKWqWYiL
Nhy6DWPEub/53JZhsHOW5OGHYJWqoZslnvDMPWdV7VdZJ3QDHpdi7vhlNR8xxQcY
nqiJ2XqqL1LeDlyfqhWbS456tZornTlhG2OnvzafvJRpYxykHeMj/Sh9FsUCgYAy
kLJ7mlNL5+CB0lycwmCgl2ddz7K8ggt/jgYz9f27JOsOWbtKQn71OZx20gbHpuvV
XYrgUIme7y+i3phfdGR1B5zlpjE5C+oG8udXP8I0PKAagy4a8j0CNqJtJZaVwtEk
3EeWg5vO/MCu7Hl2j3zWEEgoe7IJ6w2qjLghRxhrQQKBgQCiAnVt7AAIokPYBF9X
wLvUq9MZvk+kbRmRulIKKDSjRAGlxPK3wG416xUfEEox/wmuQicWsb83SOqrSX6Z
vFixHmPWpvv8a0r26gmdHyhMmXTcl2P5RHdJ/iCBKj9/pIAabhenN/vClnsB8vv/
tN9fcep4jGpay5xZ0Nj2fSWygw=="
                };
                moduleConfig = new MangoServiceConfiguration
                {
                    ValidIssuer = "test.cn",
                    ValidAudience = "mango.usercenter",
                    DbConnectString = "server=localhost;database=mytest;user=root;password=228887",
                    RedisConnectString = "localhost:6379,password=228887"
                };
            }
            else
            {
                //生产环境
                var token = Configuration["Consul:Token"];
                var configKey = Configuration["Service:ConfigKey"];
                var consulIp = Configuration["Consul:Ip"];
                var consulPort = Configuration["Consul:Port"];

                var config = new MangoConfig($"http://{consulIp}:{consulPort}", token);

                moduleConfig = config.GetConfig(configKey).Result;
                globalConfig = config.GetConfig("mango/global").Result;
            }
            #endregion

            #region 授权配置

            var policyKeyPair = new Dictionary<string, string>
            {
                {"client",moduleConfig.ValidAudience },
                {"admin","mango.admin" }
            };

            services.AddMangoJwtPolicy(policyKeyPair, opt =>
            {
                opt.Key = globalConfig.JwtKey;
                opt.Issuer = moduleConfig.ValidIssuer;
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
                op.ConnectionString = moduleConfig.RedisConnectString;
            });

            services.AddMangoDbContext<BlogDbContext, BlogOfWork>(moduleConfig.DbConnectString);

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

            services.AddHostedService<ArticleJobService>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (Env.IsDevelopment())
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

            #region 服务注册
            if (Env.IsProduction())
            {
                var token = Configuration["Consul:Token"];
                var consulIp = Configuration["Consul:Ip"];
                var consulPort = Configuration["Consul:Port"];
                var serviceName = Configuration["Service:Name"];
                var servicePort = Configuration["Service:Port"];
                var healthCheck = Configuration["Service:HealthCheck"];
                var currentIp = Configuration["Service:Ip"];

                var rc = new ConsulRegistration($"http://{consulIp}:{consulPort}", token)
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
