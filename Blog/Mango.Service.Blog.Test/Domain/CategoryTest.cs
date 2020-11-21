using Mango.Service.Blog.Domain.AggregateModel.CategoryAggreate;
using Mango.Service.Blog.Domain.AggregateModel.Enum;
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
    }
}
