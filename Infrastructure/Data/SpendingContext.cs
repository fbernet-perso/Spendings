﻿using System.Globalization;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    /// <summary>
    /// EF Db context for spending management
    /// </summary>
    public class SpendingContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Spending> Spendings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot config = configurationBuilder.Build();

            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(config["ConnectionString"]);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureModel(modelBuilder);

            SeedData(modelBuilder);
        }

        private void ConfigureModel(ModelBuilder modelBuilder)
        {
            // Spending composite PK on User, Date, Amount
            modelBuilder.Entity<Spending>()
              .HasKey(d => new { d.UserId, d.DateInUtc, d.Amount });
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasData(User.SeedUsers());           
        }

    }
}
