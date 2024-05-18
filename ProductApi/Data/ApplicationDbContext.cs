using ProductApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            for (int i = 1; i <= 5; i++)
            {
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    Id = i,
                    Name = "Product Name " + i.ToString(),
                    Price = i * 100,
                    Description = "Description for Product " + i.ToString(),
                    ImageUrl = "https://placeholder.co/603x403",
                    Category = i % 2 == 0 ? "Category 2" : "Category 1",
                });
            }
        }
    }
}
