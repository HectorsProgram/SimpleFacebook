using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpleFacebook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SimpleFacebook.Data;


namespace SimpleFacebook.Controllers;

[Authorize]
public class HomeController : Controller
{
    // private readonly ILogger<HomeController> _logger;

    // public HomeController(ILogger<HomeController> logger)
    // {
    //     _logger = logger;
    // }

    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
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

        // Ensure each post's user has a profile picture path set
        foreach (var post in posts)
        {
            if (post.User != null && string.IsNullOrEmpty(post.User.ProfilePicturePath))
            {
                post.User.ProfilePicturePath = "/uploads/profile-pictures/default.png";
            }
        }
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

    // Upload a profile picture
    [HttpPost]
    public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
    {
        if (profilePicture != null && profilePicture.Length > 0)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile-pictures");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var fileName = $"user_{user.Id}_" + Path.GetFileName(profilePicture.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }
            user.ProfilePicturePath = "/uploads/profile-pictures/" + fileName;
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
