using System.Security.Cryptography.X509Certificates;

namespace CyberBilby.MgmtClient.Components.Models;

public class Identity
{
    public string? Name { get; set; }
    public X509Certificate2? Certificate { get; set; }
}
