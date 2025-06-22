using SimpleFacebook.Models;
using SimpleFacebook.Models.Friend;
using SimpleFacebook.Data;
using Microsoft.EntityFrameworkCore;

namespace SimpleFacebook.Services;

public interface INotificationService
{
    Task SendFriendRequestNotificationAsync(int senderId, int receiverId);

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

    public async Task SendFriendRequestNotificationAsync(int senderId, int receiverId)
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


}