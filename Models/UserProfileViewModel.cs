using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SimpleFacebook.Models
{
    public class UserProfileViewModel
    {
        public int ProfileOwnerId { get; set; }
        // public string ProfileOwnerName { get; set; } = string.Empty;
        // public string ProfileOwnerImageUrl { get; set; } = string.Empty;

        public List<PostWithCommentsViewModel> ProfileOwnerPosts { get; set; }
        // public bool IsFriend { get; set; }

        public int? FriendshipId { get; set; }
        public bool IsFriendRequestSent { get; set; }
        public bool IsAddedByOwner { get; set; }
        public bool IsFriend { get; set; }
    }
}