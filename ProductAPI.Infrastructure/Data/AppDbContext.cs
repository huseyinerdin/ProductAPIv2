using Microsoft.EntityFrameworkCore;
using ProductAPI.Core.Entities;

namespace ProductAPI.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(p =>
            {
                p.HasKey(x => x.Id);
                p.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                p.Property(x => x.Description)
                    .HasMaxLength(500);
                p.Property(x => x.Price)
                    .IsRequired()
                    .HasPrecision(18, 2);
            });
        }
        public DbSet<Product> Products { get; set; }
    }
}
