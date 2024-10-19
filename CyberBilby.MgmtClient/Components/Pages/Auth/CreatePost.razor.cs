using CyberBilby.MgmtClient.Components.Models;
using CyberBilby.MgmtClient.Services;
using CyberBilby.Shared.Database.Entities;
using Microsoft.AspNetCore.Components;

namespace CyberBilby.MgmtClient.Components.Pages.Auth;

public partial class CreatePost : ComponentBase
{
    [Inject]
    private ManagementService ManagementService { get; set; } = default!;

    [Inject]
    private AlertQueue AlertQueue { get; set; } = default!;

    private string? title;
    private string? slug;
    private string? shortContent;
    private string? content;

    protected override void OnInitialized()
    {
        ManagementService.CreatePostResponse += ManagementService_CreatePostResponse;
    }

    private void ManagementService_CreatePostResponse(object? sender, Events.CreatePostResponseEventArgs e)
    {
        AlertQueue.Push(e.Result ? AlertType.Info : AlertType.Error, e.Message);
    }

    private async Task CreatePostAsync()
    {
        if(string.IsNullOrEmpty(slug))
        {
            AlertQueue.Push(AlertType.Error, "You must supply a slug.");
            return;
        }

        if (string.IsNullOrEmpty(title))
        {
            AlertQueue.Push(AlertType.Error, "You must supply a title.");
            return;
        }

        if (string.IsNullOrEmpty(shortContent))
        {
            AlertQueue.Push(AlertType.Error, "You must supply short content.");
            return;
        }

        if (string.IsNullOrEmpty(content))
        {
            AlertQueue.Push(AlertType.Error, "You must supply content.");
            return;
        }

        AlertQueue.Push(AlertType.Info, "Sending create post request", 1000);

        await ManagementService.RequestCreatePostAsync(new BlogPost()
        {
            Slug = slug,
            Title = title,
            ShortContent = shortContent,
            Content = content,
            Author = null
        });
    }
}
