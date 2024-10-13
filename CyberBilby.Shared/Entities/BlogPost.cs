namespace CyberBilby.Shared.Entities;

public class BlogPost
{
    public int Id { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }

    public BlogAuthor Author { get; set; }

    public BlogPost()
    {
        Title = string.Empty;
        Content = string.Empty;

        Author = new BlogAuthor();
    }
}
