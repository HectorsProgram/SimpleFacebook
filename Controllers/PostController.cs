using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleFacebook.Data;
using SimpleFacebook.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleFacebook.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        // Feed page
        public IActionResult Index()
        {
            // Check if user is logged in
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "User");
            }

            var posts = _context.Posts.Include(p => p.User).OrderByDescending(p => p.CreatedAt).ToList();
            return View(posts);
        }

        // Create a new post
        [HttpPost]
        public async Task<IActionResult> Create(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var post = new Post
                {
                    Content = content,
                    CreatedAt = DateTime.Now,
                    UserId = HttpContext.Session.GetInt32("UserId").Value // Replace with the logged-in user ID once authentication is set up
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
