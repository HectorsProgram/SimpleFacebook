using SimpleFacebook.Models;
using SimpleFacebook.Models.Friend;
using SimpleFacebook.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SimpleFacebook.Services
{
    public interface IFriendService
    {

        Task SendFriendRequestAsync(int senderId, int receiverId);
        Task AcceptFriendRequestAsync(int requestId);
        Task RejectFriendRequestAsync(int requestId);
        Task UnfriendUser(int? requestId);
        Task<bool> AreFriendsAsync(int userId1, int userId2);
        Task<IEnumerable<Friendships>> GetPendingRequestsAsync(int userId);
        Task<bool> IsFriendRequestPendingAsync(int senderId, int receiverId);
        Task<int> GetFriendshipIdAsync(int user, int friend);
        // Task<IEnumerable<User>> GetFriendsAsync(int userId);
        // Task<int> GetRequestId(int requestId);
    }

    public class FriendService : IFriendService
    {
        private readonly AppDbContext _context;

        public FriendService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Implement the methods here...

        public async Task SendFriendRequestAsync(int senderId, int receiverId)
        {
            var friendRequest = new Friendships
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Status = "Pending",
                // SentAt = DateTime.UtcNow
            };

            _context.Friendships.Add(friendRequest);

            // var notification = new Notification
            // {
            //     SenderId = senderId,
            //     ReceiverId = receiverId,
            //     Message = "sent you a friend request.",
            //     Type = "FriendRequest",
            //     CreatedAt = DateTime.UtcNow
            // };

            // _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();
        }

        public async Task RejectFriendRequestAsync(int requestId)
        {
            var request = await _context.Friendships
                .FirstOrDefaultAsync(r => r.Id == requestId && r.Status == "Pending");

            if (request == null) return;

            _context.Friendships.Remove(request);

            await _context.SaveChangesAsync();
        }

        public async Task UnfriendUser(int? requestId)
        {
            var request = await _context.Friendships
                .FirstOrDefaultAsync(r => r.Id == requestId && r.Status == "Accepted");
                
            if (request == null) return;

            _context.Friendships.Remove(request);

            await _context.SaveChangesAsync();

        }

        public async Task AcceptFriendRequestAsync(int requestId)
        {
            var request = await _context.Friendships
                .Include(r => r.Sender)
                .Include(r => r.Receiver)
                .FirstOrDefaultAsync(r => r.Id == requestId && r.Status == "Pending");

            if (request == null) return;

            request.Status = "Accepted";

            // var notification = new Notification
            // {
            //     SenderId = request.ReceiverId,
            //     ReceiverId = request.SenderId,
            //     Message = "accepted your friend request.",
            //     Type = "FriendRequestAccepted",
            //     CreatedAt = DateTime.UtcNow
            // };

            // _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Friendships>> GetPendingRequestsAsync(int userId)
        {
            return await _context.Friendships
                .Include(r => r.Sender)
                .Where(r => r.ReceiverId == userId && r.Status == "Pending")
                .ToListAsync();
        }

        public async Task<bool> AreFriendsAsync(int userId, int friendUserId)
        {
            return await _context.Friendships.AnyAsync(f =>
                ((f.SenderId == userId && f.ReceiverId == friendUserId) ||
                (f.SenderId == friendUserId && f.ReceiverId == userId)) &&
                f.Status == "Accepted");
        }

        public async Task<bool> IsFriendRequestPendingAsync(int senderId, int receiverId)
        {
            return await _context.Friendships.AnyAsync(r =>
                r.SenderId == senderId && r.ReceiverId == receiverId && r.Status == "Pending");
        }

        public async Task<int> GetFriendshipIdAsync(int user, int friend)
        {
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f =>
                    (f.SenderId == user && f.ReceiverId == friend) ||
                    (f.SenderId == friend && f.ReceiverId == user));

            return friendship?.Id ?? 0;
        }

    }
}
