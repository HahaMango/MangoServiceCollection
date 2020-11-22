using Mango.EntityFramework.Abstractions;
using Mango.Service.Blog.Domain.AggregateModel.ArticleAggregate;
using Mango.Service.Blog.Domain.AggregateModel.CategoryAggreate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Domain.Service
{
    /// <summary>
    /// 文章领域服务
    /// </summary>
    public class ArticleDomainService
    {
        private readonly IArticleRepository _articleRepository;

        private readonly ICategoryRepository _categoryRepository;

        private IUnitOfWork UnitOfWork => _articleRepository.UnitOfWork;

        public ArticleDomainService(
            IArticleRepository articleRepository,
            ICategoryRepository categoryRepository)
        {
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// 创建文章（自动创建一个默认分类）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<Article> CreateArticleNoCategory(long userId, string title, string desc, string content)
        {
            var category = await _categoryRepository.QueryUserDefaultCategoryAsync(userId);
            if (category == null)
            {
                category = new Category(userId, "默认分类");
                category.SetDefault();
            }
            await _categoryRepository.AddAsync(category);

            var article = new Article(userId, title, desc, content, new List<Category>
            {
                category
            });
            await _articleRepository.AddAsync(article);

            await UnitOfWork.SaveChangesAsync();

            return article;
        }
    }
}
