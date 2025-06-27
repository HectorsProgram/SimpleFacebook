
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleFacebook.Models
{

    public class Media
    {
        public int Id { get; set; }
        public string MediaUrl { get; set; }
        public MediaType Type { get; set; } // Enum: Image, Video

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }

    public enum MediaType
    {
        Image,
        Video
    }
}

