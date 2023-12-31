﻿using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
	public class BlogPostCommentRepository : IBlogPostCommentRepository
	{
		private readonly BlogDbContext _blogDbContext;

		public BlogPostCommentRepository(BlogDbContext blogDbContext)
        {
			this._blogDbContext = blogDbContext;
		}
        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
		{
			await _blogDbContext.BlogPostComments.AddAsync(blogPostComment);
			await _blogDbContext.SaveChangesAsync();
			return blogPostComment;
		}

		public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
		{
			return await _blogDbContext.BlogPostComments
				.Where(x => x.BlogPostId == blogPostId)
				.ToListAsync();
		}
	}
}
