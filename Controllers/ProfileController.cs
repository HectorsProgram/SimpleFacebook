using Microsoft.AspNetCore.Mvc;
using SimpleFacebook.Data;
using SimpleFacebook.Models;
using SimpleFacebook.Services;
using System.Linq;
using System.Net.Mime;

public class ProfileController : Controller
{
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;

    public ProfileController(IPostService postService,
        ICommentService commentService)
    {
        _postService = postService ?? throw new System.Exception("IPostService not registered");
        _commentService = commentService ?? throw new System.Exception("ICommentService not registered");
    }

    /// <summary>
    /// Displays the profile page for the specified user.
    /// If no userId is provided, shows the currently logged-in user's profile.
    /// </summary>
    /// <param name="userId">The user's ID (optional).</param>
    /// <returns>The profile view with user and posts.</returns>
    /// 
    [HttpGet]
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


        // Get posts for this user
        var posts = _postService.GetPosts(userId);

        var postsWithComments = posts.Select(post => new PostWithCommentsViewModel
        {
            Post = post,
            Comments = _commentService.GetCommentsByPost(post.PostId)
        }).ToList();

        // Pass user and posts to the view using a ViewModel or ViewBag
        // ViewBag.User = user;
        // ViewBag.Posts = posts;

        return View(postsWithComments);
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