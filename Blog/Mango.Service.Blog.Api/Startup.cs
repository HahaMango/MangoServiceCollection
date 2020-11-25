using AspectCore.Extensions.DependencyInjection;
using Mango.Core.Authentication.Extension;
using Mango.Core.Config;
using Mango.Core.Converter;
using Mango.Core.Dapper.Extension;
using Mango.Core.DataStructure;
using Mango.Core.Extension;
using Mango.EntityFramework.Extension;
using Mango.Service.Blog.Domain.AggregateModel.ArticleAggregate;
using Mango.Service.Blog.Domain.AggregateModel.BloggerAggreate;
using Mango.Service.Blog.Domain.AggregateModel.CategoryAggreate;
using Mango.Service.Blog.Domain.AggregateModel.CommentAggreate;
using Mango.Service.Blog.Domain.Service;
using Mango.Service.Blog.Infrastructure.DbContext;
using Mango.Service.Blog.Infrastructure.Repositories;
using Mango.Service.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Mango.Service.Blog.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
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

            #region ≈‰÷√÷––ƒ
            var moduleConfig = default(MangoServiceConfiguration);
            var globalConfig = default(MangoServiceConfiguration);
            if (Env.IsDevelopment())
            {
                //≤‚ ‘ª∑æ≥
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
                    ValidAudience = "mango.blog",
                    DbConnectString = "server=localhost;database=mytest;user=root;password=228887",
                    RedisConnectString = "localhost:6379,password=228887"
                };
            }
            else
            {
                //…˙≤˙ª∑æ≥
                var token = Configuration["Consul:Token"];
                var configKey = Configuration["Service:ConfigKey"];
                var consulIp = Configuration["Consul:Ip"];
                var consulPort = Configuration["Consul:Port"];

                var config = new MangoConfig($"http://{consulIp}:{consulPort}", token);

                moduleConfig = config.GetConfig(configKey).Result;
                globalConfig = config.GetConfig("mango/global").Result;
            }
            #endregion

            #region  ⁄»®≈‰÷√

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

            #region øÁ”Ú≈‰÷√

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

            #region ≤÷¥¢≈‰÷√

            services.AddMangoDbContext<BlogDbContext>(moduleConfig.DbConnectString);
            services.AddDapper(moduleConfig.DbConnectString, typeof(MySqlConnection));

            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IBloggerRepository, BloggerRepository>();

            #endregion

            #region ∑˛ŒÒ≈‰÷√

            services.AddAutoMapper();
            services.AddMediatR();
            //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TryCatchBehaviors<,>));
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<ArticleDomainService>();
            #endregion

            #region AOP≈‰÷√
            services.ConfigureDynamicProxy();
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

            app.UseHttpsRedirection();

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
