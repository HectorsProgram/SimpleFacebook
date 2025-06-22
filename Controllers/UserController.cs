using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SimpleFacebook.Data;
using SimpleFacebook.Models;
using System.Security.Claims;
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
            if (string.IsNullOrEmpty(user.ProfilePicturePath))
            {
                user.ProfilePicturePath = "/uploads/profile-pictures/default.png";
            }
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // Login page
        // [HttpGet]
        // public IActionResult Login()
        // {
        //     HttpContext.Session.Remove("UserId");
        //     return View();
        // }

        // Handle Login
        [HttpGet]
        public IActionResult Login(string email, string password)
        {
            // AUTO LOGIN AS ADMIN FOR DEVELOPMENT/TESTING PURPOSES
            // Uncomment the following block to always log in as the first admin user:
            // /*
            var adminUser = _context.Users.FirstOrDefault(u => u.Roles == "Admin");
            if (adminUser != null)
            {
                HttpContext.Session.SetInt32("UserId", adminUser.Id);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, adminUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, adminUser.Username),
                    new Claim(ClaimTypes.Role, adminUser.Roles)
                };
                // Console.WriteLine($"Auto-logging in as admin: {adminUser.Id.ToString()} - {adminUser.Username}");
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();
                return RedirectToAction("Index", "Home");
            }
            // */
            var user = _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                // Store user ID in session to keep track of logged-in user
                HttpContext.Session.SetInt32("UserId", user.Id);

                // Ensure user.Roles is not null
                if (string.IsNullOrEmpty(user.Roles))
                {
                    user.Roles = "User";
                    _context.SaveChanges();
                }

                // Set up claims for authentication
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Roles)

                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(principal).Wait();

                return RedirectToAction("Index", "Home"); // Redirect to post feed
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
