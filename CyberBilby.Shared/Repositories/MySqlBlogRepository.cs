using CyberBilby.Shared.Database;
using CyberBilby.Shared.Database.Entities;
using CyberBilby.Shared.Database.Models;

using Microsoft.EntityFrameworkCore;

namespace CyberBilby.Shared.Repositories;

public class MySqlBlogRepository : IBlogRepository
{
    private readonly BilbyDbContext dbContext;

    public MySqlBlogRepository(BilbyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CreatePostAsync(BlogPost post)
    {
        await dbContext.Posts.AddAsync(post);

        var rows = await dbContext.SaveChangesAsync();
        if(rows < 0)
        {
            throw new Exception("Failed to create post, no rows were written.");
        }
    }

    public Task DeletePostAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<BlogPost>> GetAllPostsAsync()
    {
        return await dbContext.Posts.ToListAsync();
    }

    public async Task<List<ShortBlogPost>> GetAllShortPostsAsync()
    {
        return await dbContext.Posts.Select(p => new ShortBlogPost()
        {
            Id = p.Id,
            Slug = p.Slug,
            Title = p.Title,
            ShortContent = p.ShortContent,
            Author = p.Author,
        }).ToListAsync();
    }

    public async Task<BlogAuthor?> GetAuthorAsync(string fingerprint)
    {
        return await dbContext.Authors.FirstOrDefaultAsync(a => a.Fingerprint.ToLower() == fingerprint.ToLower());
    }

    public async Task<BlogPost?> GetPostByIdAsync(int id)
    {
        return await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> IsCertificateRevokedAsync(string fingerprint)
    {
        return await dbContext.RevokedCertificates
            .AnyAsync(c => c.Fingerprint.ToLower() == fingerprint.ToLower());
    }

    public async Task<bool> PostWithSlugExistsAsync(BlogPost post)
    {
        return await dbContext.Posts.FirstOrDefaultAsync(p => p.Slug.ToLower() == post.Slug.ToLower()) is not null;
    }

    public async Task UpdatePostAsync(BlogPost post)
    {
        //dbContext.Posts.FirstOrDefaultAsync
    }
}
