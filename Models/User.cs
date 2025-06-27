using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleFacebook.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "First name must contain only letters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Last name must contain only letters.")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string ProfilePicturePath { get; set; } = "/uploads/profile-pictures/default.png";

        public int FriendCount { get; set; } = 0;
        public string Roles { get; set; } = "User"; // Default role
        //public ICollection<Post> Posts { get; set; }
    }
}
