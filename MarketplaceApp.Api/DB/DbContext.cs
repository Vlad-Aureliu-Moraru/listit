namespace MarketplaceApp.Api.DB;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Api.Models;

public class MarketplaceDbContext : DbContext
{
    public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options)
        : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<UserProfile> UserProfile { get; set; }
    public DbSet<Category> Category { get; set; } 
    public DbSet<Announcement> Announcement { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        // =========================================================
        // FIX: Force Singular Table Names to match your SQLite DB
        // =========================================================
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<UserProfile>().ToTable("UserProfile");
        modelBuilder.Entity<Category>().ToTable("Category");          // Fixes 'no such table: Categories'
        modelBuilder.Entity<Announcement>().ToTable("Announcement");  // Fixes potential plural issue here too

        // --- 1. User Achievement ---
        modelBuilder.Entity<UserAchievement>()
            .HasKey(ua => new { ua.UserId, ua.AchievementId });

        modelBuilder.Entity<UserAchievement>()
            .HasOne(ua => ua.User)
            .WithMany()
            .HasForeignKey(ua => ua.UserId)
            .IsRequired();
        
        // --- 2. User <-> UserProfile ---
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserProfile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // --- 3. Announcement Relationships ---
        
        // A. Relationship: Announcement -> User
        modelBuilder.Entity<Announcement>()
            .HasOne(a => a.User)
            .WithMany(u => u.Announcements) 
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // B. Relationship: Announcement -> Category
        modelBuilder.Entity<Announcement>()
            .HasOne(a => a.Category)
            .WithMany(c => c.Announcements) 
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}