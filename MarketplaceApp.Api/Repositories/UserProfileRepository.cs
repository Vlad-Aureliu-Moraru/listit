using Dapper;
using MarketplaceApp.Api.DB;
using MarketplaceApp.Api.Models;

namespace MarketplaceApp.Api.Repositories;

public class UserProfileRepository
{
    private readonly MarketplaceDbContext _context;

    public UserProfileRepository(MarketplaceDbContext context)
    {
        _context = context;
    }


    public UserProfile? GetByUserId(int userId)
    {
        return _context.UserProfile
            .FirstOrDefault(p => p.UserId == userId);
    }

    public void Update(UserProfile profile)
    {
        _context.UserProfile.Update(profile);
        _context.SaveChanges();
    }

    public bool ProfileExists(int userId)
    {
        return _context.UserProfile.Any(p => p.UserId == userId);
    }
}