using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.DataLayer.Domain;

namespace MyShop.DataLayer.Context
{
    public class MyShopDbContext:DbContext
    {
        public MyShopDbContext(DbContextOptions<MyShopDbContext> options):base(options)
        {
                
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }


    }
}
