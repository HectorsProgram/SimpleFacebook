using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SimpleFacebook.Models;

namespace SimpleFacebook.Models.Friend
{
    public class Friendships
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        public User Sender { get; set; } = null!;

        [Required]
        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }
        public User Receiver { get; set; } = null!;

        [Required]
        public DateTime SentTime { get; set; } = DateTime.UtcNow;
        // public DateTime FriendedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected
    }
}
