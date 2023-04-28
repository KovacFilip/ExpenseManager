using DB.models;
using Helper.Helpers;

namespace DB.Utilities
{
    public static class DatabaseInitializer
    {
        internal static void Seed(ExpensesDbContext context)
        {
            var users = new List<Person>
            {
                new Person { Username = "Admin", Role = Roles.ADMIN, },
                new Person { Username = "Filip", Role = Roles.USER, }
            };

            context.People.AddRange(users);
            context.SaveChanges();

            var hashes = new List<PasswordHash>
            {
                new PasswordHash
                {
                    PersonId = users[0].Id,
                    Hash = HelperFunctions.HashPassword("Admin"),
                },
                new PasswordHash
                {
                    PersonId = users[1].Id,
                    Hash = HelperFunctions.HashPassword("Filip"),
                }
            };

            context.PasswordHashes.AddRange(hashes);
            context.SaveChanges();

            var expenses = new List<Expense>
            {
                new Expense
                {
                    Category = "Beer",
                    Price = 200,
                    PersonId = users[1].Id,
                },
                new Expense
                {
                    Category = "Food",
                    Price = 150,
                    PersonId = users[1].Id,
                },
                new Expense
                {
                    Category = "Electronics",
                    Price = 1500,
                    PersonId = users[1].Id,
                },
            };

            context.Expenses.AddRange(expenses);
            context.SaveChanges(); // save changes to the database
        }

        public static void ResetDb()
        {
            using (var db = new ExpensesDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Seed(db);
            }
        }
    }
}
