using Mango.Core.Cache.Extension;
using Mango.Core.Extension;
using Mango.EntityFramework.Extension;
using Mango.Service.Blog.Abstractions.Repositories;
using Mango.Service.Blog.Abstractions.Services;
using Mango.Service.Blog.Job;
using Mango.Service.Blog.Repositories;
using Mango.Service.Blog.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mango.Service.Blog
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
            services.AddMangoRedis(op =>
            {
                op.ConnectionString = Configuration["RedisConnection"];
            });

            services.AddMangoDbContext<BlogDbContext, BlogOfWork>("server=localhost;database=mangosystem;user=root;password=228887");

            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleDetailRepository, ArticleDetailRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IArticleCacheService, ArticleCacheService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IJobService, JobService>();

            services.AddHostedService<ArticleJobService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
