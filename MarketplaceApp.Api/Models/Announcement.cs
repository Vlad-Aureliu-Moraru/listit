namespace MarketplaceApp.Api.Models;

public class Announcement
{
    public int AnnouncementId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public DateTime PostedDate { get; set; }
    public int CategoryId { get; set; }
    public int UserId { get; set; }
}