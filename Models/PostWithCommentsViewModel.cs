namespace SimpleFacebook.Models
{
    public class PostWithCommentsViewModel
    {
        public int? CurrentUserId { get; set; }
        public Post Post { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}