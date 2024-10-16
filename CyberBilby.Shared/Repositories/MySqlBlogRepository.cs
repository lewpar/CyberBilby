using CyberBilby.Shared.Database;
using CyberBilby.Shared.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace CyberBilby.Shared.Repositories;

public class MySqlBlogRepository : IBlogRepository
{
    private readonly BilbyDbContext dbContext;

    public MySqlBlogRepository(BilbyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task AddPostAsync(BlogPost post)
    {
        throw new NotImplementedException();
    }

    public Task DeletePostAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BlogPost>> GetAllPostsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<BlogAuthor?> GetAuthorAsync(string fingerprint)
    {
        return await dbContext.Authors.FirstOrDefaultAsync(a => a.Fingerprint.ToLower() == fingerprint.ToLower());
    }

    public Task<BlogPost?> GetPostByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsCertificateRevokedAsync(string fingerprint)
    {
        return await dbContext.RevokedCertificates
            .AnyAsync(c => c.Fingerprint.ToLower() == fingerprint.ToLower());
    }

    public Task UpdatePostAsync(BlogPost post)
    {
        throw new NotImplementedException();
    }
}
