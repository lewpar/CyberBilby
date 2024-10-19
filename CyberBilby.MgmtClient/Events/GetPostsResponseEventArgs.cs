using CyberBilby.Shared.Database.Entities;

namespace CyberBilby.MgmtClient.Events;

public class GetPostsResponseEventArgs : EventArgs
{
    public List<BlogPost> Posts { get; set; }

    public GetPostsResponseEventArgs(List<BlogPost> posts)
    {
        Posts = posts;
    }
}
