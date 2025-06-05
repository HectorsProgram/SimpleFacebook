using Microsoft.AspNetCore.Mvc;
using SimpleFacebook.Controllers;
using SimpleFacebook.Models;
using SimpleFacebook.Services;

namespace SimpleFacebook.Controllers.Shared
{
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        // private readonly IPostService _postService;

        public CommentsController(ICommentService commentService, IPostService postService)
        {
            _commentService = commentService ?? throw new System.Exception("ICommentService not registered");
            // _postService = postService ?? throw new System.Exception("IPostService not registered");
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, string content, string returnAction, string returnController)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            // console

            var comment = new Comment
            {
                PostId = postId,            // <- the post being commented on
                UserId = userId.Value,      // <- the user commenting
                Content = content,
                CreatedAt = DateTime.Now
            };


            _commentService.AddComment(comment);
            return RedirectToAction(returnAction, returnController);
        }

    }
}