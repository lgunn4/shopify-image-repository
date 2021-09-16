using Microsoft.EntityFrameworkCore;
using shopify_image_repository.Models;

namespace shopify_image_repository.Data
{
    public class UserImageDbContext : DbContext
    {
        public UserImageDbContext(DbContextOptions<UserImageDbContext> options) :
            base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Image>().ToTable("Image");
        }
    }
}