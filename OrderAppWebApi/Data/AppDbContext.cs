using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using OrderAppWebApi.Models;

namespace OrderAppWebApi.Data {

    public class AppDbContext : DbContext {
    
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) {
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Orderline> Orderlines { get; set; }
        public DbSet<Salesperson> Salespeople { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Customer>(e => {
                e.HasIndex(c => c.Code).IsUnique(true);
            });
        }
    }
}
