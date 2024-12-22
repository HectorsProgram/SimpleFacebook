
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
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        //public ICollection<Post> Posts { get; set; }
    }
}
