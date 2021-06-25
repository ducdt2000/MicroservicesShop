using Mango.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.ProductAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 1,
                Name = "Shoes 1",
                Price = 2000,
                CategoryName = "Shoes",
                Description = "Hello world and hehe",
                ImageUrl = "https://cf.shopee.vn/file/a7c893d688c8430db3f118c00319fc9a"
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 2,
                Name = "Shoes 2",
                Price = 2000,
                CategoryName = "Shoes",
                Description = "Hello world and hehe 222 ",
                ImageUrl = "https://giayxshop.vn/wp-content/uploads/2021/01/z2261641090407_9a527dfa37fa44a1b8cfd0fd14d1ca77-scaled.jpg"
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 3,
                Name = "Ball hehe",
                Price = 2000,
                CategoryName = "Ball",
                Description = "Manchester United",
                ImageUrl = "https://www.hidosport.com/uploads/5/4/1/0/54105107/manchester-united-18-19-home-kit-2-3_orig.jpg"
            });
        }
    }
}
