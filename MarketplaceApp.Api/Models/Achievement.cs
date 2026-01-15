using System.ComponentModel.DataAnnotations;

namespace MarketplaceApp.Api.Models;

public class Achievement
{
    [Key]
    public int AchievementId { get; set; }

    [Required] // Name cannot be empty
    [MaxLength(100)] // Limit length to 100 chars
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;

    // Use 'ImageUrl' so it's clear this is a string path (e.g., "/assets/badges/gold.png")
    public string? ImageUrl { get; set; }
}