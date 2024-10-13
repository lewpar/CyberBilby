using CyberBilby.MgmtClient.Components.Models;
using CyberBilby.MgmtClient.Services;

using CyberBilby.Shared.Security;

using Microsoft.AspNetCore.Components;
using System.Security.Cryptography.X509Certificates;

namespace CyberBilby.MgmtClient.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject]
    public AlertQueue AlertQueue { get; set; } = default!;

    [Inject]
    public ManagementService ManagementService { get; set; } = default!;

    public string? Host { get; set; }
    public int? Port { get; set; }

    public List<Identity> Identities { get; set; } = new List<Identity>();

    protected override async Task OnInitializedAsync()
    {
        await LoadIdentitiesAsync();
    }

    public async Task ConnectAsync()
    {
        if (string.IsNullOrEmpty(Host))
        {
            AlertQueue.Push(AlertType.Error, "You must supply a hostname to connect to.");
            return;
        }

        if (Port is null || Port < 1 || Port > 65535)
        {
            AlertQueue.Push(AlertType.Error, "You must supply a valid port.");
            return;
        }

        try
        {
            await ManagementService.ConnectToMgmtServerAsync(Host, Port.Value, 
                new System.Security.Cryptography.X509Certificates.X509Certificate2());
        }
        catch(Exception ex)
        {
            AlertQueue.Push(AlertType.Error, $"Failed to connect: {ex.Message}");
        }
    }

    private async Task LoadIdentitiesAsync()
    {
        Identities.Clear();

        var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

        store.Open(OpenFlags.ReadOnly);

        var certs = store.Certificates.Where(c => c.Issuer == "CN=LIME Intermediate");

        foreach(var cert in certs)
        {
            Identities.Add(new Identity()
            {
                Name = cert.Subject,
                Certificate = cert
            });
        }
    }
}
