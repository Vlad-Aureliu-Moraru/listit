
using System.ComponentModel.DataAnnotations;
namespace MarketplaceApp.Api.Models.AnnouncementDTOs;
public class AnnouncementResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime PostedDate { get; set; }
    
    // Flattened Category Info
    public string CategoryName { get; set; }
    
    // Flattened Seller Info (Safe, no passwords)
    public int SellerId { get; set; }
    public string SellerName { get; set; }
    public string? SellerAvatar { get; set; }
    public string? SellerPhone { get; set; }
}