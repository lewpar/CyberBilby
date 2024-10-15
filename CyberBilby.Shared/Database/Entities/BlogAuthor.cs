using CyberBilby.Shared.Network;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace CyberBilby.Shared.Database.Entities;

[PrimaryKey(nameof(Id))]
[Table("authors")]
public class BlogAuthor
{
    [Column("id")]
    public required int Id { get; set; }

    [Column("name")]
    public required string Name { get; set; }

    [Column("role")]
    public required AuthRole Role { get; set; }

    [Column("fingerprint")]
    public required string Fingerprint { get; set; }
}
