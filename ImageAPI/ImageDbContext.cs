using Microsoft.EntityFrameworkCore;
namespace ImageAPI
{
    public class ImageDbContext : DbContext
    {
        public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options)
        {
        }

        public DbSet<Models.Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Image>().ToTable("UploadedImages");
        }

    }
}
