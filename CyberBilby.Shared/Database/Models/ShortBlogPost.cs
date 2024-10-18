using CyberBilby.Shared.Database.Entities;

namespace CyberBilby.Shared.Database.Models;

public class ShortBlogPost
{
    public required int Id { get; set; }
    public required string Slug { get; set; }
    public required string Title { get; set; }
    public required string ShortContent { get; set; }
    public required BlogAuthor Author { get; set; }
}
