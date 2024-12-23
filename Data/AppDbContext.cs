using Microsoft.EntityFrameworkCore;
using SimpleFacebook.Models;

namespace SimpleFacebook.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; } // Optional
    }
}
