using CyberBilby.Shared.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace CyberBilby.Shared.Database;

public class BilbyDbContext : DbContext
{
    public DbSet<BlogAuthor> Authors { get; set; }
    public DbSet<BlogPost> Posts { get; set; }
    public DbSet<RevokedCertificate> RevokedCertificates { get; set; }

    public BilbyDbContext(DbContextOptions<BilbyDbContext> options) : base(options) { }
}
