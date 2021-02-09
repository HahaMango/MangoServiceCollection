using Mango.Service.Blog.Domain.AggregateModel.ArticleDataAggregate;
using Xunit;

namespace Mango.Service.Blog.Test.Domain
{
    public class ArticleDataTest
    {
        [Fact]
        public void CreateArticleData()
        {
            var articleData = new ArticleData(123, 32, 34, 6);
            
            Assert.NotNull(articleData);
            Assert.Equal(123,articleData.Id);
            Assert.Equal(32,articleData.View);
            Assert.Equal(34,articleData.Comment);
            Assert.Equal(6,articleData.Like);
        }

        [Fact]
        public void DoLikeTest()
        {
            var articleData = new ArticleData(123, 32, 34, 6);
            
            articleData.IncLike(1);
            
            Assert.Equal(1,articleData.LikeInc);
            Assert.Equal(7,articleData.Like);
        }

        [Fact]
        public void DoCommentTest()
        {
            var articleData = new ArticleData(123, 32, 34, 6);
            
            articleData.IncComment(-4);
            
            Assert.Equal(-4,articleData.CommentInc);
            Assert.Equal(30,articleData.Comment);
        }

        [Fact]
        public void DoViewTest()
        {
            var articleData = new ArticleData(123, 32, 34, 6);
            
            articleData.IncView(4);
            
            Assert.Equal(4,articleData.ViewInc);
            Assert.Equal(36,articleData.View);
        }
    }
}