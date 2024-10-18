using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace CyberBilby.Shared.Database.Entities;

[PrimaryKey(nameof(Id))]
[Table("posts")]
public class BlogPost
{
    [Column("id")]
    public required int Id { get; set; }

    [Column("slug")]
    public required string Slug { get; set; }

    [Column("title")]
    public required string Title { get; set; }

    [Column("content")]
    public required string Content { get; set; }

    [Column("short_content")]
    public required string ShortContent { get; set; }

    [ForeignKey(nameof(AuthorId))]
    [Column("author_id")]
    public required int AuthorId { get; set; }

    public required BlogAuthor Author { get; set; }
}
