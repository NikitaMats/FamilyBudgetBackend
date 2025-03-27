using System;
using System.Reflection;
using FamilyBudgetBackend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyBudgetBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }

        private class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.HasKey(u => u.Id);
                builder.Property(u => u.Name).IsRequired();
                builder.Property(u => u.Email).IsRequired();
                builder.HasMany(u => u.Transactions)
                       .WithOne(t => t.User)
                       .HasForeignKey(t => t.UserId)
                       .OnDelete(DeleteBehavior.Cascade);
            }
        }

        private class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionType>
        {
            public void Configure(EntityTypeBuilder<TransactionType> builder)
            {
                builder.HasKey(tt => tt.Id);
                builder.Property(tt => tt.Name).IsRequired();
                builder.HasMany(tt => tt.Categories)
                       .WithOne(c => c.TransactionType)
                       .HasForeignKey(c => c.TransactionTypeId)
                       .OnDelete(DeleteBehavior.Restrict);
            }
        }

        private class CategoryConfiguration : IEntityTypeConfiguration<Category>
        {
            public void Configure(EntityTypeBuilder<Category> builder)
            {
                builder.HasKey(c => c.Id);
                builder.Property(c => c.Name).IsRequired();
            }
        }

        private class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
        {
            public void Configure(EntityTypeBuilder<Transaction> builder)
            {
                builder.HasKey(t => t.Id);
                builder.Property(t => t.Amount).IsRequired();
                builder.Property(t => t.Date).IsRequired();
                builder.Property(t => t.Description).IsRequired();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=familybudget.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Применяем все конфигурации автоматически
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Начальные данные остаются здесь для простоты
            modelBuilder.Entity<TransactionType>().HasData(
                new TransactionType { Id = 1, Name = "Доход" },
                new TransactionType { Id = 2, Name = "Расход" }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Зарплата", TransactionTypeId = 1 },
                new Category { Id = 2, Name = "Инвестиции", TransactionTypeId = 1 },
                new Category { Id = 3, Name = "Еда", TransactionTypeId = 2 },
                new Category { Id = 4, Name = "Транспорт", TransactionTypeId = 2 }
            );
        }
    }
}
