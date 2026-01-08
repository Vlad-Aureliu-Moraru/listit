namespace MarketplaceApp.Api.Models;

public class UserProfile
{
    public int UserProfileId { get; set; }
    public int UserId { get; set; }

    public string? Description { get; set; }
    public string? ProfilePictureUrl { get; set; }
}