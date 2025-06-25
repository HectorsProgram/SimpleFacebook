using Microsoft.AspNetCore.Mvc;
using SimpleFacebook.Models;
using SimpleFacebook.Services;


namespace SimpleFacebook.Controllers.Shared;
public class PostsController : Controller
{
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;

    public PostsController(IPostService postService, ICommentService commentService)
    {
        _postService = postService ?? throw new System.Exception("IPostService not registered");
        _commentService = commentService ?? throw new System.Exception("ICommentService not registered");
    }

    // Feed page
    public IActionResult Index()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        // Check if user is logged in
        if (userId == null)
        {
            return RedirectToAction("Login", "User");
        }

        // Get posts for this user
        var posts = _postService.GetPosts(null);

        var postsWithComments = posts.Select(post => new PostWithCommentsViewModel
        {
            Post = post,
            Comments = _commentService.GetCommentsByPost(post.Id),
        }).ToList();

        return View(postsWithComments);
    }
}