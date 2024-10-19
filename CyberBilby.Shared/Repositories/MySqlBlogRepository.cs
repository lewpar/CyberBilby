﻿using CyberBilby.Shared.Database;
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

    public Task AddPostAsync(BlogPost post)
    {
        throw new NotImplementedException();
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
