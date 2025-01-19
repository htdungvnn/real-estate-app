using Microsoft.EntityFrameworkCore;
using SearchService.Models;

namespace SearchService.Data
{
    public class SearchDbContext : DbContext
    {
        public SearchDbContext(DbContextOptions<SearchDbContext> options) : base(options) { }

        public DbSet<SearchMetadata> SearchMetadata { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchMetadata>().HasKey(s => s.Id);
        }
    }
}