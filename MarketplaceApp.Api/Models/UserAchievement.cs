using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Api.Models;

// This class links a User to an Achievement
public class UserAchievement
{
    public int UserId { get; set; }
    public User? User { get; set; }

    public int AchievementId { get; set; }
    public Achievement? Achievement { get; set; }

    // Optional: Date they earned it
}