using Mango.Service.Blog.Domain.AggregateModel.CommentAggreate;
using Mango.Service.Blog.Domain.AggregateModel.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using Mango.Service.Blog.Domain.Event;

namespace Mango.Service.Blog.Test.Domain
{
    public class CommentTest
    {
        [Fact]
        public void CreateCommentTest()
        {
            var comment = new Comment(123, "mango", 456, "测试内容");

            var domainEvent = comment.DomainEvents.FirstOrDefault();

            Assert.NotNull(comment);
            Assert.Equal(123, comment.BloggerInfo.UserId);
            Assert.Equal("mango", comment.BloggerInfo.UserName);
            Assert.Equal("测试内容", comment.Content);
            Assert.IsType<CreateCommentEvent>(domainEvent);
        }

        [Fact]
        public void CreateCommentNoContentTest()
        {
            Assert.Throws<ArgumentNullException>(() => new Comment(123, "mango", 456, ""));
        }

        [Fact]
        public void CreateCommentNullContentTest()
        {
            Assert.Throws<ArgumentNullException>(() => new Comment(123, "mango", 456, null));
        }

        [Fact]
        public void CreateCommentReplyMainCommentTest()
        {
            var mainComment = new Comment(123, "mango", 456, "测试内容");
            var replyComment = new Comment(123, "mango", 456, "测试评论回复", mainComment);

            Assert.Equal(1, replyComment.IsSubComment);
            Assert.Equal(mainComment.Id, replyComment.ReplyMainCommentId);
        }

        [Fact]
        public void CreateCommentReplySubCommentTest()
        {
            var mainComment = new Comment(123, "mango", 456, "测试内容");
            var subComment = new Comment(890, "chiva", 456, "测试评论回复", mainComment);
            var replyComment = new Comment(123, "mango", 456, "测试回复子评论", subComment);

            Assert.Equal(1, replyComment.IsSubComment);
            Assert.Equal(mainComment.Id, replyComment.ReplyMainCommentId);
            Assert.Equal(subComment.Id, replyComment.ReplySubCommentId);
            Assert.Equal(890, replyComment.ReplySubBloggerInfo.UserId);
            Assert.Equal("chiva", replyComment.ReplySubBloggerInfo.UserName);
        }

        [Fact]
        public void DeleteCommentTest()
        {
            var comment = new Comment(123, "mango", 456, "测试内容");
            comment.Delete();

            Assert.Equal(EntityStatusEnum.Deleted, comment.Status);
        }
    }
}
