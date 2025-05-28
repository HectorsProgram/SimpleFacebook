using Microsoft.AspNetCore.Mvc;
using SimpleFacebook.Data;

public class ProfileController : Controller
{
    private readonly AppDbContext _context;

    public ProfileController(AppDbContext context)
    {
        _context = context;
    }

    // View user profile
    public IActionResult Index()
    // public IActionResult Index(int userId)
    {
        // var user = _context.Users.Find(userId);
        // if (user == null)
        // {
        //     return NotFound();
        // }

        // // Ensure the profile picture path is set
        // if (string.IsNullOrEmpty(user.ProfilePicturePath))
        // {
        //     user.ProfilePicturePath = "/uploads/profile-pictures/default.png";
        // }

        return View();
        // return View(user);
    }
}