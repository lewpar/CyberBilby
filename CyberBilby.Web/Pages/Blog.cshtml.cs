using CyberBilby.Shared.Database.Models;
using CyberBilby.Shared.Repositories;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CyberBilby.Web.Pages;

public class BlogModel : PageModel
{
    public List<ShortBlogPost> Posts { get; set; } = new List<ShortBlogPost>();

    private IBlogRepository BlogRepository { get; set; }

    public BlogModel(IBlogRepository blogRepository)
    {
        BlogRepository = blogRepository;
    }

    public async Task OnGetAsync()
    {
        Posts = await BlogRepository.GetAllShortPostsAsync();
    }
}
