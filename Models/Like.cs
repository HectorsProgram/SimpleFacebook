using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleFacebook.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Username { get; set; }

        [Required]
        public int PostId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }
}
