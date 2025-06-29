using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<ProductSize> ProductSizes => Set<ProductSize>();
        public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
        public DbSet<ProductSubCategory> ProductSubCategories => Set<ProductSubCategory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<ProductSubCategory>().HasKey(psc => new { psc.ProductId, psc.CategoryId });

            // modelBuilder.Entity<Category>()
            //     .HasOne(c => c.ParentCategory)
            //     .WithMany()
            //     .HasForeignKey(c => c.ParentCategoryId)
            //     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductSubCategory>().HasKey(psc => new { psc.ProductId, psc.CategoryId });

            // Configure Category
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure ProductSize
            modelBuilder.Entity<ProductSize>()
                .ToTable("ProductSizes")
                .HasKey(ps => ps.Id);

            // Configure many-to-many relationship between Product and ProductSize
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Sizes)
                .WithMany()
                .UsingEntity(j => j.ToTable("ProductProductSizes"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
