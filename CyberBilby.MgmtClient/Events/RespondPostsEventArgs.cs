using CyberBilby.Shared.Database.Entities;

namespace CyberBilby.MgmtClient.Events;

public class RespondPostsEventArgs : EventArgs
{
    public List<BlogPost> Posts { get; set; }

    public RespondPostsEventArgs(List<BlogPost> posts)
    {
        Posts = posts;
    }
}
