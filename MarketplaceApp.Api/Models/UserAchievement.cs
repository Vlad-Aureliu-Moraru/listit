using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Api.Models;

public class UserAchievement {
        public int Id { get; set; }
    
        // This MUST be the same type as User.Id (int)
        public int UserId { get; set; } 
    
        [ForeignKey("UserId")]
        public User User { get; set; }
}