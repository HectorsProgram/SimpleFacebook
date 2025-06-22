using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SimpleFacebook.Data;
using SimpleFacebook.Models;

namespace SimpleFacebook.Services
{
    public interface ICommentService
    {
        IEnumerable<Comment> GetCommentsByPost(int postId);
        Task<List<Comment>> GetCommentsByUserPosts(List<int> postIds);
        Comment? GetCommentById(int commentId);
        void AddComment(Comment comment);
        void DeleteComment(int commentId);
    }

    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;

        public CommentService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Comment> GetCommentsByPost(int postId)
        {
            return _context.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.CreatedAt)
                .ToList();
        }

        public async Task<List<Comment>> GetCommentsByUserPosts(List<int> postIds)
        {
            return await _context.Comments
                .Where(c => postIds.Contains(c.PostId))
                .ToListAsync();
        }
        public Comment? GetCommentById(int commentId)
        {
            return _context.Comments
                .Include(c => c.User)
                .FirstOrDefault(c => c.Id == commentId);
        }

        public void AddComment(Comment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void DeleteComment(int commentId)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
        }
    }
}
