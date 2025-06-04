using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpleFacebook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SimpleFacebook.Data;
using SimpleFacebook.Services;


namespace SimpleFacebook.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;

    public HomeController(IPostService postService,
        ICommentService commentService)
    {
        _postService = postService ?? throw new System.Exception("IPostService not registered");
        _commentService = commentService ?? throw new System.Exception("ICommentService not registered");
    }

    // Feed page
    public IActionResult Index()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        // Check if user is logged in
        if (HttpContext.Session.GetInt32("UserId") == null)
        {
            return RedirectToAction("Login", "User");
        }
        // Get posts for this user
        var posts = _postService.GetPosts(null);

        var postsWithComments = posts.Select(post => new PostWithCommentsViewModel
        {
            Post = post,
            Comments = _commentService.GetCommentsByPost(post.PostId),
        }).ToList();

        // Pass user and posts to the view using a ViewModel or ViewBag
        // ViewBag.User = user;
        // ViewBag.Posts = posts;

        return View(postsWithComments);
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

            _postService.AddPost(post);
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }
    /*
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
    */


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
