using CyberBilby.Shared.Database.Entities;

namespace CyberBilby.MgmtClient.Events;

public class RequestPostsResponseEventArgs : EventArgs
{
    public List<BlogPost> Posts { get; set; }

    public RequestPostsResponseEventArgs(List<BlogPost> posts)
    {
        Posts = posts;
    }
}
