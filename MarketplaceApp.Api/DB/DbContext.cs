namespace MarketplaceApp.Api.DB;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Api.Models;

public class MarketplaceDbContext : DbContext
{
    public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options)
        : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet <UserProfile> UserProfile { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Always call base first

        modelBuilder.Entity<UserAchievement>()
            .HasKey(ua => new { ua.UserId, ua.AchievementId });

        modelBuilder.Entity<UserAchievement>()
            .HasOne(ua => ua.User)
            .WithMany() // If User does NOT have a list of achievements, keep this empty.
            .HasForeignKey(ua => ua.UserId)
            .IsRequired(); // Make sure UserId is required
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserProfile)      // A User has one Profile
            .WithOne(p => p.User)            // A Profile belongs to one User
            .HasForeignKey<UserProfile>(p => p.UserId) // The Foreign Key is in the UserProfile table
            .OnDelete(DeleteBehavior.Cascade); // If User is deleted, Profile is deleted t 
    }}


