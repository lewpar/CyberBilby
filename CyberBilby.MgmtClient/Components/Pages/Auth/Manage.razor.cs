using CyberBilby.MgmtClient.Components.Models;
using CyberBilby.MgmtClient.Services;

using Microsoft.AspNetCore.Components;

namespace CyberBilby.MgmtClient.Components.Pages.Auth;

public partial class Manage : ComponentBase
{
    [Inject]
    public ProfileManager ProfileManager { get; set; } = default!;

    public AuthPage CurrentPage { get; set; } = AuthPage.Dashboard;

    public void UpdatePage(AuthPage page)
    {
        CurrentPage = page;
    }
}
