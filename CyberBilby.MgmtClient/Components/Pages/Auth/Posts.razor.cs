﻿using CyberBilby.MgmtClient.Events;
using CyberBilby.MgmtClient.Services;

using CyberBilby.Shared.Database.Entities;

using Microsoft.AspNetCore.Components;

namespace CyberBilby.MgmtClient.Components.Pages.Auth;

public partial class Posts : ComponentBase
{
    [Inject]
    private ManagementService Manage { get; set; } = default!;

    private List<BlogPost>? posts;
    private bool postsLoaded;

    protected override void OnInitialized()
    {
        Manage.GetPostsResponse += OnRequestPostsResponseReceived;
    }

    protected override async Task OnParametersSetAsync()
    {
        await Task.Delay(500);

        await Manage.RequestGetPostsAsync();
    }

    private async void OnRequestPostsResponseReceived(object? sender, GetPostsResponseEventArgs e)
    {
        await this.InvokeAsync(() =>
        {
            posts = e.Posts;
            postsLoaded = true;

            StateHasChanged();
        });
    }
}