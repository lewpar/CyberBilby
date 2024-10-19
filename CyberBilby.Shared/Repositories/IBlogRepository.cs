using CyberBilby.Shared.Database.Entities;
using CyberBilby.Shared.Database.Models;

namespace CyberBilby.Shared.Repositories;

public interface IBlogRepository
{
    Task<List<BlogPost>> GetAllPostsAsync();
    Task<List<ShortBlogPost>> GetAllShortPostsAsync();

    Task<BlogPost?> GetPostByIdAsync(int id);
    Task CreatePostAsync(BlogPost post);
    Task UpdatePostAsync(BlogPost post);
    Task DeletePostAsync(int id);

    Task<bool> PostWithSlugExistsAsync(BlogPost post);

    Task<BlogAuthor?> GetAuthorAsync(string fingerprint);

    Task<bool> IsCertificateRevokedAsync(string fingerprint);
}
