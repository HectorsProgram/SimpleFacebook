using Microsoft.EntityFrameworkCore;
using SimpleFacebook.Models;
using SimpleFacebook.Models.Friend;

namespace SimpleFacebook.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; } // Optional, for notifications
        public DbSet<Like> Likes { get; set; } // Optional
        public DbSet<Friendships> Friendships { get; set; } // For friend requests
        public DbSet<Media> Media { get; set; } // For media files in posts
    }
}
