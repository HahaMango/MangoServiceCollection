/*--------------------------------------------------------------------------
//
//  Copyright 2020 Chiva Chen
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//
/*--------------------------------------------------------------------------*/

using Mango.Service.Blog.Domain.AggregateModel.ArticleAggregate;
using Mango.Service.Blog.Domain.AggregateModel.BloggerAggreate;
using Mango.Service.Blog.Domain.AggregateModel.CategoryAggreate;
using Mango.Service.Blog.Domain.AggregateModel.CommentAggreate;
using Mango.Service.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Mango.Service.Blog.Infrastructure.DbContext
{
    public class BlogDbContext: DefalutDbContext
    {
        public DbSet<Article> Articles { get; set; }

        public DbSet<CategoryAssociation> CategoryAssociations { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Blogger> Bloggers { get; set; }

        public BlogDbContext() { }

        public BlogDbContext(DbContextOptions options, IMediator mediator) : base(options, mediator)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ArticleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryAssociationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CommentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BloggerEntityConfiguration());
        }
    }

    /// <summary>
    /// 文章实体配置
    /// </summary>
    internal class ArticleEntityConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("article");
            builder.Property<long>("_userId").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("UserId");
            builder.Property<DateTime>("_createTime").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("CreateTime");
            builder.Property<DateTime?>("_updateTime").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("UpdateTime");
            builder.Property<int>("_view").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("View");
            builder.Property<int>("_comment").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("Comment");
            builder.Property<int>("_like").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("Like");

            //var navigation = builder.Metadata.FindNavigation(nameof(Article.Categories));
            //navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            //关于支持字段，EF会按照默认的命名规则寻找只读属性的支持字段，一般来说按照命名规格是不需要上面的代码配置，
            //而且上面的代码配置不够灵活，外键和删除模式等都无法配置。
            //在遵守支持字段的命名规则的情况下使用下面的代码来配置一对多关系会更加的灵活，代码清晰

            builder
                .HasMany(c => c.Categories)
                .WithOne()
                .HasForeignKey(c => c.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            var ai = builder.OwnsOne(p => p.ArticleInfo);
            ai.Property(a => a.Title).HasColumnName("Title");
            ai.Property(a => a.Describe).HasColumnName("Describe");

            builder.Property(a => a.Content).HasColumnName("Content");
            builder.Property(a => a.IsTop).HasColumnName("IsTop");
            builder.Property(a => a.Status).HasColumnName("Status");
        }
    }

    /// <summary>
    /// 文章分类关联实体配置
    /// </summary>
    internal class CategoryAssociationEntityConfiguration : IEntityTypeConfiguration<CategoryAssociation>
    {
        public void Configure(EntityTypeBuilder<CategoryAssociation> builder)
        {
            builder.ToTable("category_association");
            builder.Property(ca => ca.ArticleId).HasColumnName("ArticleId");
            builder.Property(ca => ca.CategoryId).HasColumnName("CategoryId");
        }
    }

    /// <summary>
    /// 分类实体配置
    /// </summary>
    internal class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category");
            builder.Property(a => a.UserId).HasColumnName("UserId");
            builder.Property<DateTime>("_createTime").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("CreateTime");
            builder.Property<DateTime?>("_updateTime").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("UpdateTime");
            builder.Property(a => a.Status).HasColumnName("Status");
            builder.Property(a => a.IsDefault).HasColumnName("IsDefault");
            builder.Property(a => a.CategoryName).HasColumnName("CategoryName");
        }
    }

    /// <summary>
    /// 评论实体配置
    /// </summary>
    internal class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comment");
            builder.Property<long>("_articleId").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("ArticleId");
            builder.Property(c => c.Status).HasColumnName("Status");

            var ui = builder.OwnsOne(c => c.UserInfo);
            ui.Property(u => u.UserId).HasColumnName("UserId");
            ui.Property(u => u.UserName).HasColumnName("UserName");

            builder.Property(c => c.Content).HasColumnName("Content");
            builder.Property(c => c.IsSubComment).HasCollation("IsSubComment");
            builder.Property(c => c.ReplyMainCommentId).HasColumnName("ReplyMainCommentId");
            builder.Property(c => c.ReplySubCommentId).HasColumnName("ReplySubCommentId");

            var rui = builder.OwnsOne(c => c.ReplySubUserInfo);
            rui.Property(u => u.UserId).HasColumnName("ReplyUserId");
            rui.Property(u => u.UserName).HasColumnName("ReplyUserName");

            builder.Property<int>("_like").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("Like");
            builder.Property<int>("_reply").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("Reply");

            builder.Property<DateTime>("_createTime").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("CreateTime");
            builder.Property<DateTime?>("_updateTime").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("UpdateTime");
        }
    }

    /// <summary>
    /// 博客用户配置
    /// </summary>
    internal class BloggerEntityConfiguration : IEntityTypeConfiguration<Blogger>
    {
        public void Configure(EntityTypeBuilder<Blogger> builder)
        {
            builder.ToTable("blogger");

            builder.Property<DateTime>("_createTime").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("CreateTime");
            builder.Property<DateTime?>("_updateTime").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("UpdateTime");
        }
    }
}
