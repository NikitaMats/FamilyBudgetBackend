using System;
using System.Reflection;
using FamilyBudgetBackend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyBudgetBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Tables in DB
        public DbSet<Transaction> Transactions { get; set; }  // Transactions table
        public DbSet<User> Users { get; set; }               // Users table
        public DbSet<Category> Categories { get; set; }      // Categories table
        public DbSet<TransactionType> TransactionTypes { get; set; } // TransactionTypes table

        // Constructor (DI)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Setting up relationships between tables
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Setting up the connection "1 user → N transactions"
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)         
                .WithMany(u => u.Transactions)  
                .HasForeignKey(t => t.UserId);

            // Setting up the connection Category → Transaction Type
            modelBuilder.Entity<Category>()
                .HasOne(c => c.TransactionType)
                .WithMany(tt => tt.Categories)
                .HasForeignKey(c => c.TransactionTypeId);
        }
    }
}