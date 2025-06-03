using SimpleFacebook.Models;

namespace SimpleFacebook.Models
{
    public class PostWithCommentsViewModel
    {
        public Post Post { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}