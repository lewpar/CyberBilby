using CyberBilby.MgmtClient.Components.Models;

using Microsoft.AspNetCore.Components;

namespace CyberBilby.MgmtClient.Components.Pages.Auth;

public partial class Manage : ComponentBase
{
    public AuthPage CurrentPage { get; set; } = AuthPage.Dashboard;

    public void UpdatePage(AuthPage page)
    {
        CurrentPage = page;
    }
}
