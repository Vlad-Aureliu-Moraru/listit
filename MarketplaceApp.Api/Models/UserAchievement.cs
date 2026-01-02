namespace MarketplaceApp.Api.Models;

public class UserAchievement
{
        public int UserId { get; set; }
        public int AchievementId { get; set; }
        public DateTime DateEarned { get; set; }
}