﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PriceCalculatingProject.Areas.Identity.Data;
using System.Reflection.Emit;

namespace PriceCalculator.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<UnitType> UnitTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>().Ignore(t => t.IsBestPrice);
        builder.Entity<UnitType>().Ignore(t => t.UnitTypesList);
        builder.Entity<UnitType>().HasMany(q => q.Products).WithOne(k => k.UnitType).OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(builder);
    }
}