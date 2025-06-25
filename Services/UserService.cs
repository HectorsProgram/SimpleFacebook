
using SimpleFacebook.Models;
using SimpleFacebook.DTOs;
using Microsoft.EntityFrameworkCore;
using SimpleFacebook.Data;

namespace SimpleFacebook.Services
{
    public interface IUserService
    {
        Task UpdateUserDataAsync(int userId, EditProfileDto dto, string? profilePicturePath);
        Task<EditProfileDto> GetUserProfileAsync(int userId);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public async Task UpdateUserDataAsync(int userId, EditProfileDto dto, string? profilePicturePath)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new Exception("User not found.");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;

            if (!string.IsNullOrEmpty(profilePicturePath))
            {
                user.ProfilePicturePath = profilePicturePath;
            }

            await _context.SaveChangesAsync();
        }
        public async Task<EditProfileDto> GetUserProfileAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new Exception("User not found.");

            return new EditProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfilePicturePath = user.ProfilePicturePath
            };
        }
    }
}
