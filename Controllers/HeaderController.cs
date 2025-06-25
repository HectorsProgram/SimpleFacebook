using Microsoft.AspNetCore.Mvc;
using SimpleFacebook.Models;
using SimpleFacebook.Services;

namespace SimpleFacebook.Controllers;

public class HeaderController : Controller
{
    private readonly INotificationService _notificationService;

    public HeaderController(INotificationService notificationService)
    {
        _notificationService = notificationService ?? throw new System.Exception("NotificationService not regitered!");
    }

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

    // [HttpPost]
    public async Task<IActionResult> OpenNotifications()
    {
        // Console.WriteLine("OpenNotifications called");
        var notifications = await _notificationService.GetNotificationsAsync(HttpContext.Session.GetInt32("UserId") ?? 0);
        // This is a placeholder for the OpenNotifications functionality.
        // It could be used to display notifications for the logged-in user.
        return PartialView("_NotificationsPartial", notifications);
    }

}