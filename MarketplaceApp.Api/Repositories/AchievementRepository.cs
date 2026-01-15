using MarketplaceApp.Api.DB;
using MarketplaceApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApp.Api.Repositories;


public class AchievementRepository 
{
    private readonly MarketplaceDbContext _context;

    public AchievementRepository(MarketplaceDbContext context)
    {
        _context = context;
    }

    public List<Achievement> GetAll()
    {
        return _context.Achievement.ToList();
    }

    public Achievement? GetById(int id)
    {
        return _context.Achievement.Find(id);
    }

// Change return type to List<Achievement>
    public List<Achievement> SearchByName(string name)
    {
        return _context.Achievement
            .Where(x => x.Name.Contains(name)) 
            .ToList();
    }
    public void Create(Achievement achievement)
    {
        _context.Achievement.Add(achievement);
        _context.SaveChanges();
    }

    // This creates the link in the "UserAchievement" table
    public void AssignAchievementToUser(int userId, int achievementId)
    {
        // 1. Check if they already have it (prevent duplicates)
        var exists = _context.Set<UserAchievement>()
            .Any(ua => ua.UserId == userId && ua.AchievementId == achievementId);

        if (exists) return; // Do nothing if they already have it

        // 2. Create the link
        var link = new UserAchievement
        {
            UserId = userId,
            AchievementId = achievementId,
        };

        _context.Set<UserAchievement>().Add(link);
        _context.SaveChanges();
    }

    // This gets all badges for a specific user profile
    public List<Achievement> GetAchievementsByUser(int userId)
    {
        return _context.Set<UserAchievement>()
            .Where(ua => ua.UserId == userId)
            .Include(ua => ua.Achievement) // Join to get the actual Badge details
            .Select(ua => ua.Achievement!) // Select the badge part
            .ToList();
    }
}