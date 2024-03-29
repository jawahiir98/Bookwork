using BookWorkRazor.Models;
using Microsoft.EntityFrameworkCore;

namespace BookWorkRazor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category {Id = 1, Name = "K-Drama", DisplayOrder = 6},   
                new Category {Id = 2, Name = "Novel", DisplayOrder = 7},    
                new Category {Id = 3, Name = "Comics", DisplayOrder = 9}    
            );
        }
    }
}
