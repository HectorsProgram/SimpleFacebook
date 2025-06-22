using Microsoft.AspNetCore.Mvc;
using SimpleFacebook.Models;

namespace SimpleFacebook.Controllers;

public class HeaderController : Controller
{
    public IActionResult Index()
    {
        // Get the user ID from the session
        int? userId = HttpContext.Session.GetInt32("UserId");

        // If user is not logged in, redirect to login page
        if (userId == null)
        {
            return RedirectToAction("Login", "User");
        }

        return RedirectToAction("OpenNotifications"); // Replace with actual header content
        // return View("Index", "Home");
    }

    public async Task<IActionResult> OpenNotifications()
    {
        
        // This is a placeholder for the OpenNotifications functionality.
        // It could be used to display notifications for the logged-in user.
        return PartialView("_NotificationsPartial");
    }

}