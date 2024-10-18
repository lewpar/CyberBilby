using CyberBilby.Shared.Database.Entities;
using CyberBilby.Shared.Database.Models;
using CyberBilby.Shared.Repositories;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CyberBilby.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<ShortBlogPost> Posts { get; set; } = new List<ShortBlogPost>();

        private IBlogRepository BlogRepository { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IBlogRepository blogRepository)
        {
            _logger = logger;

            BlogRepository = blogRepository;
        }

        public async Task OnGetAsync()
        {
            Posts = await BlogRepository.GetAllShortPostsAsync();
        }
    }
}
