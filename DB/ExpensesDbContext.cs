using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB.models;
using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class ExpensesDbContext : DbContext
    {
        private string _dbPath { get; }
        public DbSet<Person> People => Set<Person>();
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<PasswordHash> PasswordHashes => Set<PasswordHash>();

        public ExpensesDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            _dbPath = Path.Join(path, "expenses.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Person entity
            modelBuilder.Entity<Person>().HasKey(p => p.Id);
            modelBuilder.Entity<Person>().Property(p => p.Id).ValueGeneratedOnAdd();

            // Configure the Expense entity
            modelBuilder.Entity<Expense>().HasKey(e => e.ObjectId);
            modelBuilder.Entity<Expense>().Property(e => e.ObjectId).ValueGeneratedOnAdd();

            modelBuilder
                .Entity<Expense>()
                .HasOne<Person>(e => e.Person)
                .WithMany(p => p.Expenses)
                .HasForeignKey(e => e.PersonId);

            // Configure the PasswordHash entity
            modelBuilder.Entity<PasswordHash>().HasKey(ph => ph.PersonId);

            modelBuilder
                .Entity<PasswordHash>()
                .HasOne<Person>(ph => ph.Person)
                .WithOne()
                .HasForeignKey<PasswordHash>(ph => ph.PersonId);
        }
    }
}
