using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MarketplaceApp.Api.Models;

[Table("User")]
public class User
{
    public UserProfile? UserProfile { get; set; }
    [Key]
    public int Id { get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required]
    [JsonIgnore]
    public string PasswordHash { get; set; }

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    
    public List<Announcement> Announcements { get; set; } 
}