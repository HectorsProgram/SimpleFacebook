using System.ComponentModel.DataAnnotations;

namespace SimpleFacebook.DTOs
{
    public class EditProfileDto
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "First name must contain only letters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Last name must contain only letters.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? ProfilePicturePath { get; set; }

    }
}
