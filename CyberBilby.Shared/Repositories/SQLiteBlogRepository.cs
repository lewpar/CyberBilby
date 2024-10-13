using CyberBilby.Shared.Entities;

using RepoDb;

using System.Data.SQLite;

namespace CyberBilby.Shared.Repositories;

public class SQLiteBlogRepository : IBlogRepository
{
    private string _connectionString;

    public SQLiteBlogRepository(string connectionString)
    {
        _connectionString = connectionString;

        GlobalConfiguration.Setup().UseSQLite();
    }

    public async Task AddPostAsync(BlogPost post)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.InsertAsync<BlogPost>(post);
        }
    }

    public async Task DeletePostAsync(int id)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.DeleteAsync<BlogPost>(p => p.Id == id);
        }
    }

    public async Task<IEnumerable<BlogPost>> GetAllPostsAsync()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            return await connection.QueryAllAsync<BlogPost>();
        }
    }

    public async Task<BlogPost?> GetPostByIdAsync(int id)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            var posts = await connection.QueryAsync<BlogPost>(p => p.Id == id);
            return posts.FirstOrDefault();
        }
    }

    public async Task UpdatePostAsync(BlogPost post)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.UpdateAsync<BlogPost>(post);
        }
    }
}
