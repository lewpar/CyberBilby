using CyberBilby.Shared.Database.Entities;

namespace CyberBilby.Shared.Repositories;

public interface IBlogRepository
{
    Task<IEnumerable<BlogPost>> GetAllPostsAsync();

    Task<BlogPost?> GetPostByIdAsync(int id);
    Task AddPostAsync(BlogPost post);
    Task UpdatePostAsync(BlogPost post);
    Task DeletePostAsync(int id);
}
