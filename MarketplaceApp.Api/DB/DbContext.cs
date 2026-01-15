namespace MarketplaceApp.Api.DB;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Api.Models;

public class MarketplaceDbContext : DbContext
{
    public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options)
        : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<UserProfile> UserProfile { get; set; }
    public DbSet<Category> Categories { get; set; } 
    public DbSet<Announcement> Announcements { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        // --- 1. User Achievement (Composite Key) ---
        modelBuilder.Entity<UserAchievement>()
            .HasKey(ua => new { ua.UserId, ua.AchievementId });

        modelBuilder.Entity<UserAchievement>()
            .HasOne(ua => ua.User)
            .WithMany()
            .HasForeignKey(ua => ua.UserId)
            .IsRequired();
        
        // --- 2. User <-> UserProfile (One-to-One) ---
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserProfile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // --- 3. Announcement Relationships (NEW) ---
        
        // A. Relationship: Announcement -> User (Seller)
        modelBuilder.Entity<Announcement>()
            .HasOne(a => a.User)
            .WithMany() // Assuming User doesn't have a "List<Announcement>" property yet
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade); // If User is deleted, delete their ads

        // B. Relationship: Announcement -> Category
        modelBuilder.Entity<Announcement>()
            .HasOne(a => a.Category)
            .WithMany() 
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // PREVENTS deleting a Category if ads exist
    }
}