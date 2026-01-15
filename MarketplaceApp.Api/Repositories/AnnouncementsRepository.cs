using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Api.DB;
using MarketplaceApp.Api.Models;

namespace MarketplaceApp.Api.Repositories;

public class AnnouncementsRepository 
{
    private readonly MarketplaceDbContext _context;

    public AnnouncementsRepository(MarketplaceDbContext context)
    {
        _context = context;
    }

    public List<Announcement> GetAll()
    {
        return _context.Announcement
            .Include(a => a.Category)
            // 2. Join with User (to get "Thorn")
            .Include(a => a.User)
            // 3. Go deeper: Join User -> UserProfile (to get the seller's PFP)
                .ThenInclude(u => u.UserProfile)
            .ToList();
    }
    // Inside AnnouncementRepository.cs
    public List<Announcement> SearchByTitle(string title)
    {
        return _context.Announcement
            .Include(a => a.Category) // Don't forget Includes for Announcements!
            .Include(a => a.User)
            .Where(x => x.Title.Contains(title)) 
            .ToList();
    }

    public Announcement? GetById(int id)
    {
        return _context.Announcement
            .Include(a => a.Category)
            .Include(a => a.User)
                .ThenInclude(u => u.UserProfile)
            .FirstOrDefault(a => a.AnnouncementId == id);
    }

    public List<Announcement> GetByCategoryId(int categoryId)
    {
        return _context.Announcement
            .Include(a => a.User) // We still need seller info
                .ThenInclude(u => u.UserProfile)
            .Where(a => a.CategoryId == categoryId)
            .ToList();
    }
    
    public List<Announcement> GetByUserId(int userId)
    {
        return _context.Announcement
            .Include(a => a.Category) // We need category info
            .Where(a => a.UserId == userId)
            .ToList();
    }

    public void Create(Announcement announcement)
    {
        _context.Announcement.Add(announcement);
        _context.SaveChanges();
    }

    public void Update(Announcement announcement)
    {
        _context.Announcement.Update(announcement);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var item = _context.Announcement.Find(id);
        if (item != null)
        {
            _context.Announcement.Remove(item);
            _context.SaveChanges();
        }
    }
}