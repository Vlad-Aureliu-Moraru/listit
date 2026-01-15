using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // Needed for [JsonIgnore]

namespace MarketplaceApp.Api.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(50)] 
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Description { get; set; }
    
    // --- RELATIONSHIP ---
    // This connects the Category to your Announcements table.
    // It allows you to easily find all items in a category (e.g., category.Announcements).
    
    [JsonIgnore] // Critical: Prevents the "Cycle Detected" crash when fetching categories
    public List<Announcement> Announcements { get; set; } = new();
}