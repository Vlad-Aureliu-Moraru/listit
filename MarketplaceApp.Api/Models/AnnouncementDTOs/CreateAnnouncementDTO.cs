using System.ComponentModel.DataAnnotations;
namespace MarketplaceApp.Api.Models.AnnouncementDTOs;
public class CreateAnnouncementDTO
{
    [Required]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    [Range(0, double.MaxValue)]
    public double Price { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
    
    [Required]
    public int UserId { get; set; } // The ID of the person posting
    
    public string? ImageUrl { get; set; }
}