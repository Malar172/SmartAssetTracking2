using Microsoft.EntityFrameworkCore;

namespace AssetTracking
{
    public class AssetDbContext : DbContext
    {
        public DbSet<Asset> Assets { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=localhost\SQLEXPRESS;
                  Database=AssetDB;
                  Trusted_Connection=True;
                  TrustServerCertificate=True");
        }
    }
}