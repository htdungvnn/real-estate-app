using Microsoft.EntityFrameworkCore;
using PropertyService.Models;

namespace PropertyService.Data
{
    public class PropertyDbContext : DbContext
    {
        public PropertyDbContext(DbContextOptions<PropertyDbContext> options) : base(options) { }

        public DbSet<Property> Properties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>().HasKey(p => p.Id);
            modelBuilder.Entity<Property>().Property(p => p.Title).IsRequired();
        }
    }
}