﻿using CouponApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CouponApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon { Id = 1, Code="10off", DiscountAmount=10, MinAmount=20 });
            modelBuilder.Entity<Coupon>().HasData(new Coupon { Id = 2, Code="20off", DiscountAmount=20, MinAmount=40 });
        }
    }
}
