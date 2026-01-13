using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MarketplaceApp.Api.DB; // Ensure this matches your namespace

namespace MarketplaceApp.Api.DB
{
    public class MarketplaceDbContextFactory : IDesignTimeDbContextFactory<MarketplaceDbContext>
    {
        public MarketplaceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MarketplaceDbContext>();
            
            // 1. Use the exact same connection string as your Program.cs
            optionsBuilder.UseSqlite("Data Source=marketplace.db");

            // 2. Return the context directly
            return new MarketplaceDbContext(optionsBuilder.Options);
        }
    }
}