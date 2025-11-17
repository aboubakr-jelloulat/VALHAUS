using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Valhaus.Models;
using Valhaus.Models.Models;
namespace Valhaus.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData
                (
                    new Category { Id = 1, Name = "Tables", DisplayOrder = 1 },
                    new Category { Id = 2, Name = "Sofas", DisplayOrder = 2 },
                    new Category { Id = 3, Name = "Accessories", DisplayOrder = 3 }
                );

            //modelBuilder.Entity<Product>().HasData
            //    (

            //    );
        }
    }
}

