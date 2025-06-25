using Microsoft.AspNetCore.Mvc;

using SimpleFacebook.Data;
using SimpleFacebook.Models;
using SimpleFacebook.Services;
using SimpleFacebook.DTOs;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

public class ProfileController : Controller
{
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;
    private readonly IFriendService _friendService;
    private readonly IUserService _userService;
    private readonly IWebHostEnvironment _env; // ✅ add this

    public ProfileController(
        IPostService postService,
        ICommentService commentService,
        IFriendService friendService,
        IUserService userService,
        IWebHostEnvironment env) // ✅ inject here
    {
        _postService = postService ?? throw new System.Exception("IPostService not registered");
        _commentService = commentService ?? throw new System.Exception("ICommentService not registered");
        _friendService = friendService ?? throw new System.Exception("IFriendService not registered");
        _userService = userService ?? throw new System.Exception("IUserService not registered");
        _env = env ?? throw new System.Exception("IWebHostEnvironment not registered"); // ✅ store it
    }

    /// <summary>
    /// Displays the profile page for the specified user.
    /// If no userId is provided, shows the currently logged-in user's profile.
    /// </summary>
    /// <param name="userId">The user's ID (optional).</param>
    /// <returns>The profile view with user and posts.</returns>
    /// 
    [HttpGet]
    public async Task<IActionResult> ProfileIndex(int? userId = null)
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
        // var allComments = _commentService.GetCommentsByUserPosts(posts.Select(p => p.PostId).ToList());
        // var groupedComments = allComments.GroupBy(c => c.PostId).ToDictionary(g => g.Key, g => g.ToList());

        //Console.WriteLine("Current user Id: "+int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value));

        var postsWithComments = posts.Select(post => new PostWithCommentsViewModel
        {
            CurrentUserId = HttpContext.Session.GetInt32("UserId"),
            // CurrentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            Post = post,
            Comments = _commentService.GetCommentsByPost(post.Id)
        }).ToList();

        int currentUserId = HttpContext.Session.GetInt32("UserId")!.Value;
        // int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        bool isFriendRequestSent = false;
        isFriendRequestSent = await _friendService.IsFriendRequestPendingAsync(currentUserId, userId.Value);

        bool isAddedByOwner = false;
        isAddedByOwner = await _friendService.IsFriendRequestPendingAsync(userId.Value, currentUserId);

        int? friendshipId = await _friendService.GetFriendshipIdAsync(currentUserId, userId.Value);

        bool isFriend = await _friendService.AreFriendsAsync(currentUserId, userId.Value);

        // Console.WriteLine(isFriend);
        var userProfileModel = new UserProfileViewModel
        {
            ProfileOwnerId = userId.Value,
            ProfileOwnerPosts = postsWithComments,
            FriendshipId = friendshipId,
            IsFriendRequestSent = isFriendRequestSent, // Default to false, will be updated below
            IsAddedByOwner = isAddedByOwner,
            IsFriend = isFriend
        };

        // Pass user and posts to the view using a ViewModel or ViewBag
        // ViewBag.User = user;
        // ViewBag.Posts = posts;

        return View(userProfileModel);
    }

    [HttpGet]
    public async Task<IActionResult> LoadEditModal()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        // int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (userId == null)
            return Unauthorized();

        var dto = await _userService.GetUserProfileAsync(userId.Value);
        return PartialView("_EditProfileModal", dto);
    }


    [HttpPost]
    public async Task<IActionResult> UpdateProfile(EditProfileDto dto, IFormFile? ProfilePicture)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_EditProfileModal", dto);
        }

        int? userId = HttpContext.Session.GetInt32("UserId");
        // int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (userId == null)
            return Unauthorized();

        string? newProfilePicPath = null;

        if (ProfilePicture != null && ProfilePicture.Length > 0)
        {
            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads/profile-pictures");
            Directory.CreateDirectory(uploadsDir);

            var fileName = Guid.NewGuid() + Path.GetExtension(ProfilePicture.FileName);
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ProfilePicture.CopyToAsync(stream);
            }

            newProfilePicPath = "/uploads/profile-pictures/" + fileName;
        }

        try
        {
            await _userService.UpdateUserDataAsync(userId.Value, dto, newProfilePicPath);
            TempData["Success"] = "Profile updated!";
            return RedirectToAction("ProfileIndex");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return PartialView("_EditProfileModal", dto);
        }
    }

    public async Task<IActionResult> UnfriendUserProfile(int? Id = null, int? RequestId = null)
    {
        // If no RequestId is provided, return BadRequest
        if (RequestId == null)
            return BadRequest("RequestId is required");

        // Unfriend the user by calling the service

        _friendService.UnfriendUser(RequestId);

        return RedirectToAction("ProfileIndex", new { userId = Id });
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RespondToRequest([FromBody] FriendRequestActionDto model)
    {

        await _friendService.AcceptFriendRequestAsync(model.RequestId);

        if (model.Action == "confirm")
        {
            return Json(new { success = true });
        }

        return BadRequest("Unknown action");
    }

    // DTO for confirming/deleting request
    public class FriendRequestActionDto
    {
        public int RequestId { get; set; }
        public string Action { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> ToggleRequest(int id)  // id = receiver's user id
    {
        int currentUserId = HttpContext.Session.GetInt32("UserId")!.Value;
        // var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (currentUserId == id)
        {
            // Can't send request to yourself
            return BadRequest("Cannot friend yourself.");
        }

        // Check if there is an existing pending friend request from current user to the target user
        bool existingRequest = await _friendService.IsFriendRequestPendingAsync(currentUserId, id);
        var existingFriendRequest = await _friendService.GetPendingRequestsAsync(id);
        var existingRequestFromCurrentUser = existingFriendRequest
            .FirstOrDefault(r => r.SenderId == currentUserId && r.ReceiverId == id);

        Console.WriteLine($"Current User ID: {currentUserId}, Target User ID: {id}");

        // Console.WriteLine($"Request ID: {existingRequestFromCurrentUser.Id}");

        if (existingRequest)
        {
            // Cancel the friend request by deleting it
            await _friendService.RejectFriendRequestAsync(existingRequestFromCurrentUser.Id);
            return Json(new { status = "notSent" });
        }

        await _friendService.SendFriendRequestAsync(currentUserId, id);
        return Json(new { status = "sent" });


        // No existing request found, so create a new one

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