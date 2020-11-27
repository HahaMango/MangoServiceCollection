using Mango.Service.Blog.Domain.AggregateModel.BloggerAggreate;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mango.Service.Blog.Test.Domain
{
    public class BloggerTest
    {
        [Fact]
        public void CreateBloggerTest()
        {
            var blogger = new Blogger(123, "测试");

            Assert.NotNull(blogger);
            Assert.Equal(123, blogger.Id);
            Assert.Equal("测试", blogger.BloggerName);
        }

        [Fact]
        public void CreateBloggerNullNameTest()
        {
            Assert.Throws<ArgumentNullException>(() => new Blogger(123, null));
        }

        [Fact]
        public void CreateBloggerEmptyNameTest()
        {
            Assert.Throws<ArgumentNullException>(() => new Blogger(123, ""));
        }

        [Fact]
        public void EditNameTest()
        {
            var blogger = new Blogger(123, "测试");

            blogger.EditName("测试1");

            Assert.Equal(123, blogger.Id);
            Assert.Equal("测试1", blogger.BloggerName);
        }

        [Fact]
        public void EditNameNullNameTest()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var b = new Blogger(123, "测试");
                b.EditName(null);
            });
        }
    }
}
