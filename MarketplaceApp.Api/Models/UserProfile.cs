using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Api.Models;

public class UserProfile
{
    [Key]
    public int UpId{ get; set; }
    
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User{ get; set; }

    [MaxLength(100)]
    public string? Description { get; set; }
    
    public string? Pfp { get; set; }
}