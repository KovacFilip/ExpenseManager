using DB.models;
using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class ExpensesDbContext : DbContext
    {
        private string _dbPath { get; }

        public ExpensesDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            _dbPath = Path.Join(path, "expenses.db");
        }

        public DbSet<Person> People => Set<Person>();
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<PasswordHash> PasswordHashes => Set<PasswordHash>();

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

            // Configure the PasswordHash entity
            modelBuilder.Entity<PasswordHash>().HasKey(ph => ph.PersonId);
        }
    }
}
