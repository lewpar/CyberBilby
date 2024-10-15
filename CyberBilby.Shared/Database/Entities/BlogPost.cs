using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace CyberBilby.Shared.Database.Entities;

[PrimaryKey(nameof(Id))]
[Table("posts")]
public class BlogPost
{
    [Column("id")]
    public required int Id { get; set; }

    [Column("title")]
    public required string Title { get; set; }

    [Column("content")]
    public required string Content { get; set; }

    [Column("author")]
    public required BlogAuthor Author { get; set; }
}
