using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleFacebook.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleFacebook.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for batch processing operations. Only authorized users can access these endpoints.
    /// </summary>

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BatchController : Controller
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchController"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public BatchController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // For demonstration: set the first user as admin if not already
            var user = _context.Users.FirstOrDefault();
            if (user != null && (string.IsNullOrEmpty(user.Roles) || user.Roles != "Admin"))
            {
                user.Roles = "Admin";
                _context.SaveChanges();
            }
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult SampleView() {
            // This is a placeholder for a sample view.
            return View();
        }
        /// <summary>
        /// Checks all users and sets the default profile picture path for those missing it.
        /// </summary>
        /// <returns>A view with the number of users updated.</returns>
        [HttpGet]
        public async Task<IActionResult> SetDefaultProfilePictures()
        {
            Console.WriteLine("Setting default profile pictures for users without one...");
            // Uncomment the following lines to actually update the database.
            var usersToUpdate = _context.Users.Where(u => string.IsNullOrEmpty(u.ProfilePicturePath)).ToList();
            foreach (var user in usersToUpdate)
            {
                user.ProfilePicturePath = "/uploads/profile-pictures/default.png";
            }
            int updatedCount = await _context.SaveChangesAsync();
            ViewBag.UpdatedCount = usersToUpdate.Count;
            return RedirectToAction("Index", "Post", new { area = "" });
        }

        /// <summary>
        /// Makes a user an admin by setting their Roles property to "Admin".
        /// </summary>
        /// <param name="userId">The ID of the user to make admin.</param>
        /// <returns>Redirects to the Index view.</returns>
        [HttpPost]
        public IActionResult MakeAdmin(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null && user.Roles != "Admin")
            {
                user.Roles = "Admin";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
