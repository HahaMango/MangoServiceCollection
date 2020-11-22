using Mango.Service.Blog.Domain.AggregateModel.ArticleAggregate;
using Mango.Service.Blog.Domain.AggregateModel.CategoryAggreate;
using Mango.Service.Blog.Domain.AggregateModel.Enum;
using System;
using System.Collections.Generic;
using Xunit;

namespace Mango.Service.Blog.Test.Domain
{
    public class ArticleTest
    {
        [Fact]
        public void CreateArticleTest()
        {
            var category = new Category(123, "测试分类");
            var article = new Article(123, "测试标题", "描述", "内容", new List<Category> { category });

            Assert.NotNull(article);
            Assert.Equal("测试标题", article.ArticleInfo.Title);
            Assert.Equal("描述", article.ArticleInfo.Describe);
            Assert.Equal("内容", article.Content);
            Assert.InRange(article.Id, 1, long.MaxValue);
            Assert.Equal(EntityStatusEnum.Available, article.Status);
        }

        [Fact]
        public void CreateArticleNoTitleTest()
        {
            var category = new Category(123, "测试分类");

            Assert.Throws<ArgumentNullException>(() => new Article(123, "", "描述", "内容", new List<Category> { category }));
        }

        [Fact]
        public void CreateArticleNullTitleTest()
        {
            var category = new Category(123, "测试分类");

            Assert.Throws<ArgumentNullException>(() => new Article(123, null, "描述", "内容", new List<Category> { category }));
        }

        [Fact]
        public void CreateArticleNoDescTest()
        {
            var category = new Category(123, "测试分类");

            Assert.Throws<ArgumentNullException>(() => new Article(123, "标题", "", "内容", new List<Category> { category }));
        }

        [Fact]
        public void CreateArticleNullDescTest()
        {
            var category = new Category(123, "测试分类");

            Assert.Throws<ArgumentNullException>(() => new Article(123, "标题", null, "内容", new List<Category> { category }));
        }

        [Fact]
        public void CreateArticleNoContentTest()
        {
            var category = new Category(123, "测试分类");

            Assert.Throws<ArgumentNullException>(() => new Article(123, "标题", "描述", "", new List<Category> { category }));
        }

        [Fact]
        public void CreateArticleNullContentTest()
        {
            var category = new Category(123, "测试分类");

            Assert.Throws<ArgumentNullException>(() => new Article(123, "标题", "描述", null, new List<Category> { category }));
        }

        [Fact]
        public void CreateArticleNullCategoriesTest()
        {
            var category = new Category(123, "测试分类");

            Assert.Throws<ArgumentException>(() => new Article(123, "标题", "描述", "内容", null));
        }

        [Fact]
        public void CreateArticleEmptyCategoriesTest()
        {
            var category = new Category(123, "测试分类");

            Assert.Throws<ArgumentException>(() => new Article(123, "标题", "描述", "内容", new List<Category>()));
        }

        [Fact]
        public void DeleteArticleTest()
        {
            var category = new Category(123, "测试分类");
            var article = new Article(123, "测试标题", "描述", "内容", new List<Category> { category });

            article.Delete();

            Assert.Equal(EntityStatusEnum.Deleted, article.Status);
        }

        [Fact]
        public void SetTopTest()
        {
            var category = new Category(123, "测试分类");
            var article = new Article(123, "测试标题", "描述", "内容", new List<Category> { category });

            article.SetTop();

            Assert.Equal(1, article.IsTop);
        }

        [Fact]
        public void CancelTopTest()
        {
            var category = new Category(123, "测试分类");
            var article = new Article(123, "测试标题", "描述", "内容", new List<Category> { category });

            article.SetTop();
            article.CancelTop();

            Assert.Equal(0, article.IsTop);
        }

        [Fact]
        public void EditAticleTest()
        {
            var category = new Category(123, "测试分类");
            var article = new Article(123, "测试标题", "描述", "内容", new List<Category> { category });

            article.Edit("标题1", "描述1", "内容1", new List<Category> { category });

            Assert.Equal("标题1", article.ArticleInfo.Title);
            Assert.Equal("描述1", article.ArticleInfo.Describe);
            Assert.Equal("内容1", article.Content);
        }

        [Fact]
        public void EditCategoryTest()
        {
            var category = new Category(123, "测试分类");
            var article = new Article(123, "测试标题", "描述", "内容", new List<Category> { category });

            var newCategory = new Category(123, "分类1");

            article.EditCategory(new List<Category> { newCategory });

            var firstCategory = article.Categories[0];
            Assert.Equal(1, article.Categories.Count);
            Assert.Equal(newCategory.Id, firstCategory.CategoryId);
            Assert.Equal(article.Id, firstCategory.ArticleId);
        }
    }
}
