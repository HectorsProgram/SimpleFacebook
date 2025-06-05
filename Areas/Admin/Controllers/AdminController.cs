using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SimpleFacebook.Data;

namespace SimpleFacebook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Admin dashboard
        public IActionResult Index()
        {
            var users = _context.Users.ToList(); // Get all users from DB
            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult PiggyBackUser(int userId)
        {
            HttpContext.Session.SetInt32("UserId", userId);

            // This is a placeholder for the PiggyBackUser functionality.
            // It could be used to impersonate another user for administrative purposes.
            return RedirectToAction("Index", "Home", new { area = "" }); // Redirect to home or any other page
        }

        [HttpPost]
        public IActionResult StopImpersonating()
        {
            HttpContext.Session.Remove("ImpersonatedUserId");
            return RedirectToAction("Index", "Home", new { area = "" }); // or wherever makes sense
        }

    }
}






