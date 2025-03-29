using System;
using System.Reflection;
using FamilyBudgetBackend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyBudgetBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Таблицы в БД
        public DbSet<Transaction> Transactions { get; set; }  // Таблица Transactions
        public DbSet<User> Users { get; set; }               // Таблица Users
        public DbSet<Category> Categories { get; set; }      // Таблица Categories
        public DbSet<TransactionType> TransactionTypes { get; set; } // Таблица TransactionTypes

        // Конструктор (DI)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Настройка связей между таблицами
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связи "1 пользователь → N транзакций"
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)         // У транзакции есть 1 пользователь
                .WithMany(u => u.Transactions)  // У пользователя много транзакций
                .HasForeignKey(t => t.UserId);  // Внешний ключ

            // Настройка связи Категория → Тип транзакции
            modelBuilder.Entity<Category>()
                .HasOne(c => c.TransactionType)
                .WithMany(tt => tt.Categories)
                .HasForeignKey(c => c.TransactionTypeId);

            // Дополнительные настройки можно добавить здесь
        }
    }
}