using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SimpleFacebook.Data;
using SimpleFacebook.Models;

namespace SimpleFacebook.Services
{
    public interface IPostService
    {
        IEnumerable<Post> GetPosts(int? userId);
        Post? GetPostById(int postId);
        void AddPost(Post post);
        // void DeletePost(int postId);
    }

    public class PostService : IPostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Post> GetPosts(int? userId)
        {
            if (userId == null)
            {
                return _context.Posts.Include(p => p.User).OrderByDescending(p => p.CreatedAt).ToList();
            }
            return _context.Posts
    .Include(p => p.User)                          // Include the related User
    .Where(p => p.UserId == userId)                // Filter by user ID
    .OrderByDescending(p => p.CreatedAt)           // Order by most recent
    .ToList();
        }

        public Post? GetPostById(int postId)
        {
            return _context.Posts.FirstOrDefault(p => p.Id == postId);
        }

        public void AddPost(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));
            _context.Posts.Add(post);
            _context.SaveChangesAsync();
        }

        public void DeletePost(int postId)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
                _context.Posts.Remove(post);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Post> GetPosts(string? userId = null)
        {
            throw new NotImplementedException();
        }
    }
}
