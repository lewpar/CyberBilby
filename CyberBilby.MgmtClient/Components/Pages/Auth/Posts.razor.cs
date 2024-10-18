using CyberBilby.MgmtClient.Services;

using CyberBilby.Shared.Database.Entities;

using Microsoft.AspNetCore.Components;

namespace CyberBilby.MgmtClient.Components.Pages.Auth;

public partial class Posts : ComponentBase
{
    [Inject]
    private ManagementService ManagementService { get; set; } = default!;

    private List<BlogPost>? posts;
    private bool postsLoaded;

    protected override void OnInitialized()
    {
        ManagementService.OnRespondPosts += ManagementService_OnRespondPosts;
    }

    private void ManagementService_OnRespondPosts(object? sender, Events.RespondPostsEventArgs e)
    {
        posts = e.Posts;
        postsLoaded = true;

        StateHasChanged();
    }

    private async Task LoadPostsAsync()
    {
        await ManagementService.RequestPostsAsync();
    }
}
