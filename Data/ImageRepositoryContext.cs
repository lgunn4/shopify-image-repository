using Microsoft.EntityFrameworkCore;
using shopify_image_repository.Models;

namespace shopify_image_repository.Data
{
    public class ImageRepositoryContext : DbContext
    {
        public ImageRepositoryContext(DbContextOptions<ImageRepositoryContext> options) :
            base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}