using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;


namespace WebApp.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {    
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
               new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
               new Category { Id = 2, Name = "Comedy", DisplayOrder = 2 },
               new Category { Id = 3, Name = "Thriller", DisplayOrder = 3 }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Author = "Stewen King",
                    Description = "A thriling horor story",
                    ISBN = "978-3-16-148410-0",
                    Price = 50,
                    Price50 = 40,
                    Price100 = 30,
                    Title="IT",
                    CategoryId=3,
                    ImageUrl=""
                },
                new Product
                {
                    Id = 2,
                    Author = "Stewen King",
                    Description = "A thriling horor story",
                    ISBN = "978-4-62-234110-0",
                    Price = 50,
                    Price50 = 40,
                    Price100 = 30,
                    Title="Kabinet of death",
                    CategoryId=1,
                    ImageUrl=""
                }
                );

        }

    }
}
