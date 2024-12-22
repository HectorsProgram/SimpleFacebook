using Microsoft.AspNetCore.Mvc;
using SimpleFacebook.Data;
using SimpleFacebook.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleFacebook.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // Registration page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Handle Registration
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            //HttpContext.Session.Remove("UserId");
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // Login page
        [HttpGet]
        public IActionResult Login()
        {
            //HttpContext.Session.Remove("UserId");
            return View();
        }

        // Handle Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                // Store user ID in session to keep track of logged-in user
                HttpContext.Session.SetInt32("UserId", user.Id);
                return RedirectToAction("Index", "Post"); // Redirect to post feed
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId"); // Clear session
            return RedirectToAction("Login");
        }
    }
}
