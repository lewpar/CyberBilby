using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace CyberBilby.Shared.Database.Entities;

[PrimaryKey(nameof(Fingerprint))]
[Table("revoked_certificates")]
public class RevokedCertificate
{
    public required string Fingerprint { get; set; }
}
