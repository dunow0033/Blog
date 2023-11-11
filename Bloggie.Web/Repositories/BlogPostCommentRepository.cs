using Bloggie.Web.Models.Domain;
using Bloggie.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories;

public class BlogPostCommentRepository: IBlogPostCommentRepository
{
    private readonly BloggieDbContext bloggieDbContext;

    public BlogPostCommentRepository(BloggieDbContext bloggieDbContext)
    {
        this.bloggieDbContext = bloggieDbContext;
    }
    
    public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
    {
        await bloggieDbContext.BlogPostComment.AddAsync(blogPostComment);
        await bloggieDbContext.SaveChangesAsync();
        return blogPostComment;
    }

    async Task<IEnumerable<BlogPostComment>> IBlogPostCommentRepository.GetCommentsByBlogIdAsync(Guid blogPostId)
    {
        return await bloggieDbContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId)
                .ToListAsync();
    }
}
