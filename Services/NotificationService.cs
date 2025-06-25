using SimpleFacebook.Models;
using SimpleFacebook.Models.Friend;
using SimpleFacebook.Data;
using Microsoft.EntityFrameworkCore;

namespace SimpleFacebook.Services;

public interface INotificationService
{
    Task AddFriendRequestNotificationAsync(int senderId, int receiverId);
    Task<List<Notification>> GetNotificationsAsync(int userId);
    // If this service already handles notifications:
    // Task SendFriendRequestNotificationAsync(int senderId, int receiverId);
}

public class NotificationService : INotificationService
{
    private readonly AppDbContext _context;

    public NotificationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddFriendRequestNotificationAsync(int senderId, int receiverId)
    {
        var friendRequest = new Friendships
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            // SentAt = DateTime.UtcNow,
            Status = "Pending"
        };

        _context.Friendships.Add(friendRequest);

        var notification = new Notification
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Message = "Sent you a friend request.",
            Type = "FriendRequest",
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Notification>> GetNotificationsAsync(int userId)
    {
        // Fetch notifications for the user
        var notifications = await _context.Notifications
            .Where(n => n.ReceiverId == userId)
            .Include(n => n.Sender) // Include sender details if needed
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        // Process or return notifications as needed
        // This could be returning a list, or processing them in some way
        // For now, we will just return the notifications
        return notifications;
    }
}