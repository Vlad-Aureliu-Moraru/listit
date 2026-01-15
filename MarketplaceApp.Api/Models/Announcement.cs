using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Api.Models;

public class Announcement
{
    [Key]
    public int AnnouncementId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(0, 1000000)] // simple validation to prevent negative prices
    public double Price { get; set; }

    public DateTime PostedDate { get; set; } = DateTime.UtcNow; // Default to 'Now'

    public string? ImageUrl { get; set; } // Suggestion: You likely need a picture for the item

    // --- Foreign Keys ---

    [Required]
    public int CategoryId { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}