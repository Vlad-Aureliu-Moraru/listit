namespace MarketplaceApp.Api.DB;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Api.Models;

public class MarketplaceDbContext : DbContext
{
    public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options)
        : base(options) { }

    public DbSet<User> User { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Always call base first

        // 1. Define the Composite Key
        modelBuilder.Entity<UserAchievement>()
            .HasKey(ua => new { ua.UserId, ua.AchievementId });

        // 2. Define the Relationship Explicitly
        modelBuilder.Entity<UserAchievement>()
            .HasOne(ua => ua.User)
            .WithMany() // If User does NOT have a list of achievements, keep this empty.
            .HasForeignKey(ua => ua.UserId)
            .IsRequired(); // Make sure UserId is required
    }}


