namespace MarketplaceApp.Api.DB;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Api.Models;

public class MarketplaceDbContext : DbContext
{
    public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
}

