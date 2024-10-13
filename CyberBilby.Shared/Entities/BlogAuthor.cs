namespace CyberBilby.Shared.Entities;

public class BlogAuthor
{
    public int Guid { get; set; }

    public string Name { get; set; }

    public BlogAuthor()
    {
        Guid = 0;

        Name = "Anonymous";
    }
}
