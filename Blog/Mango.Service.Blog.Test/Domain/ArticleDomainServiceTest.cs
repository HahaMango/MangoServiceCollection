using Mango.Service.Blog.Domain.AggregateModel.ArticleAggregate;
using Mango.Service.Blog.Domain.AggregateModel.BloggerAggreate;
using Mango.Service.Blog.Domain.AggregateModel.CategoryAggreate;
using Mango.Service.Blog.Domain.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mango.Service.Blog.Test.Domain
{
    public class ArticleDomainServiceTest
    {
        private Mock<IArticleRepository> _articleRepository;
        private Mock<ICategoryRepository> _categoryRepository;
        private Mock<IBloggerRepository> _bloggerRepository;

        public ArticleDomainServiceTest()
        {
            _articleRepository = new Mock<IArticleRepository>();
            _categoryRepository = new Mock<ICategoryRepository>();
            _bloggerRepository = new Mock<IBloggerRepository>();
        }

        [Fact]
        public async Task CreateArticleNoCategoryTest()
        {
            _articleRepository.Setup(a => a.AddAsync(It.IsAny<Article>()));
            _articleRepository.Setup(a => a.UnitOfWork.SaveChangesAsync(default))
                .Returns(Task.FromResult(1));

            _bloggerRepository.Setup(b => b.GetByIdAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(new Blogger(123, "测试用户")));

            var c = default(Category);
            _categoryRepository.Setup(c => c.QueryUserDefaultCategoryAsync(123))
                .Returns(Task.FromResult<Category>(null));
            var cs = _categoryRepository.Setup(c => c.AddAsync(It.IsAny<Category>()));
            _ = cs.Callback<Category>(cr =>
            {
                c = cr;
            });

            var service = new ArticleDomainService(_articleRepository.Object, _categoryRepository.Object, _bloggerRepository.Object);
            var result = await service.CreateArticleNoCategory(123, "标题", "描述", "内容");

            _categoryRepository.Verify(a => a.AddAsync(It.IsAny<Category>()), Times.Once());
            _articleRepository.Verify(a => a.AddAsync(It.IsAny<Article>()), Times.Once());
            _articleRepository.Verify(a => a.UnitOfWork.SaveChangesAsync(default), Times.Once());
            Assert.NotNull(result);
            Assert.NotEmpty(result.Categories);
            Assert.Equal(1, c.IsDefault);
        }

        [Fact]
        public async Task CreateArticleExistCategoryTest()
        {
            _articleRepository.Setup(a => a.AddAsync(It.IsAny<Article>()));
            _articleRepository.Setup(a => a.UnitOfWork.SaveChangesAsync(default))
                .Returns(Task.FromResult(1));

            _bloggerRepository.Setup(b => b.GetByIdAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(new Blogger(123, "测试用户")));

            var inputCategory = new Category(123, "分类");
            inputCategory.SetDefault();
            _categoryRepository.Setup(c => c.QueryUserDefaultCategoryAsync(123))
                .Returns(Task.FromResult(inputCategory));

            var service = new ArticleDomainService(_articleRepository.Object, _categoryRepository.Object, _bloggerRepository.Object);
            var result = await service.CreateArticleNoCategory(123, "标题", "描述", "内容");

            _categoryRepository.Verify(a => a.AddAsync(It.IsAny<Category>()), Times.Never());
            _articleRepository.Verify(a => a.AddAsync(It.IsAny<Article>()), Times.Once());
            _articleRepository.Verify(a => a.UnitOfWork.SaveChangesAsync(default), Times.Once());
            Assert.NotNull(result);
            Assert.NotEmpty(result.Categories);
        }

        [Fact]
        public async Task CreateArticleNoBloggerTest()
        {
            _articleRepository.Setup(a => a.AddAsync(It.IsAny<Article>()));
            _articleRepository.Setup(a => a.UnitOfWork.SaveChangesAsync(default))
                .Returns(Task.FromResult(1));

            _bloggerRepository.Setup(b => b.GetByIdAsync(It.IsAny<long>()))
                .Returns(Task.FromResult<Blogger>(null));

            var inputCategory = new Category(123, "分类");
            inputCategory.SetDefault();
            _categoryRepository.Setup(c => c.QueryUserDefaultCategoryAsync(123))
                .Returns(Task.FromResult(inputCategory));

            var service = new ArticleDomainService(_articleRepository.Object, _categoryRepository.Object, _bloggerRepository.Object);
            var result = await service.CreateArticleNoCategory(123, "标题", "描述", "内容");

            _categoryRepository.Verify(a => a.AddAsync(It.IsAny<Category>()), Times.Never());
            _articleRepository.Verify(a => a.AddAsync(It.IsAny<Article>()), Times.Never());
            _articleRepository.Verify(a => a.UnitOfWork.SaveChangesAsync(default), Times.Never());
            Assert.Null(result);
        }
    }
}
