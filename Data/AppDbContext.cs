using Microsoft.EntityFrameworkCore;
using SimpleFacebook.Models;

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
    }
}
