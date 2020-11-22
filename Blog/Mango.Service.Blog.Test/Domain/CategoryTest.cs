using Mango.Service.Blog.Domain.AggregateModel.CategoryAggreate;
using Mango.Service.Blog.Domain.AggregateModel.Enum;
using Mango.Service.Blog.Domain.Event;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mango.Service.Blog.Test.Domain
{
    public class CategoryTest
    {
        [Fact]
        public void CreateCategoryTest()
        {
            var category = new Category(123, "默认分类");

            Assert.NotNull(category);
            Assert.Equal("默认分类", category.CategoryName);
            Assert.Equal(EntityStatusEnum.Available, category.Status);
            Assert.InRange(category.Id, 1, long.MaxValue);
        }
        
        [Fact]
        public void CreateCategoryWithEmptyNameTest()
        {
            Assert.Throws<ArgumentNullException>(() => new Category(123, ""));
        }

        [Fact]
        public void CreateCategoryWithNullNameTest()
        {
            Assert.Throws<ArgumentNullException>(() => new Category(123, null));
        }

        [Fact]
        public void DeleteCategoryTest()
        {
            var category = new Category(123, "默认分类");
            category.Delete();

            Assert.NotNull(category);
            Assert.Equal(EntityStatusEnum.Deleted, category.Status);
        }

        [Fact]
        public void EditCategoryTest()
        {
            var category = new Category(123, "默认分类");
            category.Edit("编辑分类");

            Assert.NotNull(category);
            Assert.Equal("编辑分类", category.CategoryName);
        }

        [Fact]
        public void SetDefaultTest()
        {
            var category = new Category(123, "默认分类");
            category.SetDefault();

            Assert.NotNull(category);
            Assert.Equal(1, category.IsDefault);
        }

        [Fact]
        public void SetDefaultPublishDomainEvent()
        {
            var category = new Category(123, "默认分类");
            category.SetDefault();

            var domainEvent = category.DomainEvents[0];

            Assert.NotNull(category);
            Assert.IsType<SetDefaultCategoryEvent>(domainEvent);
        }

        [Fact]
        public void CancelDefaultTest()
        {
            var category = new Category(123, "默认分类");
            category.CancelDefault();

            Assert.NotNull(category);
            Assert.Equal(0, category.IsDefault);
        }
    }
}
