using Microsoft.AspNetCore.Mvc;
using SimpleFacebook.Data;
using SimpleFacebook.Models;
using SimpleFacebook.Services;
using System.Linq;
using System.Net.Mime;

public class ProfileController : Controller
{
    private readonly IPostService _postService;

    public ProfileController(IServiceProvider serviceProvider)
    {
        _postService = (IPostService?)serviceProvider.GetService(typeof(IPostService)) ?? throw new System.Exception("IPostService not registered");
    }

    /// <summary>
    /// Displays the profile page for the specified user.
    /// If no userId is provided, shows the currently logged-in user's profile.
    /// </summary>
    /// <param name="userId">The user's ID (optional).</param>
    /// <returns>The profile view with user and posts.</returns>
    public IActionResult Index(int? userId = null)
    {
        // Console.WriteLine($"ProfileController.Index called with userId: {userId}");

        // If no userId is provided, try to get the current user from session/claims
        if (userId == null)
        {
            // Example: get userId from session (adjust as needed for your auth setup)
            userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");
        }
        // return Content(userId.ToString());

        // var user = _context.Users.FirstOrDefault(u => u.Id == userId.Value);
        // if (user == null)
        // {
        //     return NotFound();
        // }

        // // Ensure the profile picture path is set
        // if (string.IsNullOrEmpty(user.ProfilePicturePath))
        // {
        //     user.ProfilePicturePath = "/uploads/profile-pictures/default.png";
        // }

        // Get posts for this user
        var posts = _postService.GetPosts(userId);

        // Pass user and posts to the view using a ViewModel or ViewBag
        // ViewBag.User = user;
        // ViewBag.Posts = posts;

        return View(posts);
    }

    /// <summary>
    /// Displays a single post by ID.
    /// </summary>
    public IActionResult Post(int postId)
    {
        var post = _postService.GetPostById(postId);
        if (post == null)
            return NotFound();

        return View(post);
    }

    /// <summary>
    /// Adds a new post for the current user.
    /// </summary>
    // [HttpPost]
    // public IActionResult AddPost(string content)
    // {
    //     var userId = HttpContext.Session.GetInt32("UserId");
    //     if (userId == null)
    //         return RedirectToAction("Login", "User");

    //     var post = new Post
    //     {
    //         UserId = userId.Value,
    //         Content = content,
    //         CreatedAt = DateTime.Now
    //     };

    //     // _postService.AddPost(post);
    //     return RedirectToAction("Index", new { userId = userId.Value });
    // }

    /// <summary>
    /// Deletes a post by ID (if owned by the current user).
    /// </summary>
    // [HttpPost]
    // public IActionResult DeletePost(int postId)
    // {
    //     var userId = HttpContext.Session.GetInt32("UserId");
    //     if (userId == null)
    //         return RedirectToAction("Login", "User");

    //     var post = _postService.GetPostById(postId);
    //     if (post == null || post.UserId != userId.Value)
    //         return Forbid();

    //     // _postService.DeletePost(postId);
    //     return RedirectToAction("Index", new { userId = userId.Value });
    // }
}