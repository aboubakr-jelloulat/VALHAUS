using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Valhaus.Models;
using Valhaus.Models.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Valhaus.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ignore identity 
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Category>().HasData
                (
                    new Category { Id = 1, Name = "Tables", DisplayOrder = 1 },
                    new Category { Id = 2, Name = "Sofas", DisplayOrder = 2 },
                    new Category { Id = 3, Name = "Accessories", DisplayOrder = 3 }
                );


            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Oslo Coffee Table",
                    Description = "Low-profile oak coffee table with rounded corners — minimalist Scandinavian design.",
                    SKU = "VH-CT-001",
                    ListPrice = 499.00,
                    Price = 449.00,
                    Price50 = 399.00,
                    Price100 = 349.00,
                    CategoryId = 1,
                    ImageUrl = ""
                },

                new Product
                {
                    Id = 2,
                    Title = "Nordic Ceramic Vase - Small",
                    Description = "Hand-glazed ceramic vase in matte white — understated elegance.",
                    SKU = "VH-VS-001",
                    ListPrice = 59.99,
                    Price = 49.99,
                    Price50 = 39.99,
                    Price100 = 29.99,
                    CategoryId = 2,
                    ImageUrl = ""
                },

                new Product
                {
                    Id = 3,
                    Title = "Nord Modular Sofa - Corner",
                    Description = "Corner modular sofa with low profile and wooden base — configurable layout.",
                    SKU = "VH-SF-002",
                    ListPrice = 3299.00,
                    Price = 2999.00,
                    Price50 = 2699.00,
                    Price100 = 2399.00,
                    CategoryId = 3,
                    ImageUrl = ""
                }
            );
        }
    }
}

