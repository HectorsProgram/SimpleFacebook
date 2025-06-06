using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SimpleFacebook.Models;

namespace SimpleFacebook.Models;

public class Notification
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Message { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public bool IsRead { get; set; } = false;

    [StringLength(50)]
    public string? Type { get; set; } // e.g., "Like", "Comment", etc.

    // Foreign Key: Receiver
    [Required]
    [ForeignKey("Receiver")]
    public int ReceiverId { get; set; }
    public User Receiver { get; set; } = null!;

    // Foreign Key: Sender (optional)
    [ForeignKey("Sender")]
    public int? SenderId { get; set; }
    public User? Sender { get; set; }

    // Optional FK to related post
}