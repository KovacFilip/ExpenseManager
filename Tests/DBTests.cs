namespace Tests;

using DB.models;
using DB.UnitOfWork;
using DB.Utilities;
using Helper.Helpers;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public async Task DatabaseReset()
    {
        DatabaseInitializer.ResetDb();
        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Admin");
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Role, Roles.ADMIN);
        Assert.IsNotNull(person.Id);

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Role, Roles.USER);
        Assert.IsNotNull(person.Id);

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Neexistuje");
        }

        Assert.IsNull(person);
    }

    [TestMethod]
    public async Task LoginCorrect()
    {
        DatabaseInitializer.ResetDb();
        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.Login("Filip", "Filip");
        }

        Assert.IsNotNull(person);
        Assert.AreEqual("Filip", person.Username);
    }

    [TestMethod]
    public async Task LoginIncorrectPassword()
    {
        DatabaseInitializer.ResetDb();

        Person? person;
        using (var uow = new UnitOfWork())
        {
            person = await uow.Login("Filip", "TohleNeniMojeHeslo");
        }

        Assert.IsNull(person);
    }

    [TestMethod]
    public async Task LoginIncorrectUsername()
    {
        DatabaseInitializer.ResetDb();

        Person? person;
        using (var uow = new UnitOfWork())
        {
            person = await uow.Login("Blbost", "Neexistuje");
        }

        Assert.IsNull(person);
    }

    [TestMethod]
    public async Task FindPersonByNameExists()
    {
        DatabaseInitializer.ResetDb();

        Person? person;
        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);
    }

    [TestMethod]
    public async Task FindPersonByNameNotExists()
    {
        DatabaseInitializer.ResetDb();

        Person? person;
        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Výmysl");
        }

        Assert.IsNull(person);
    }

    [TestMethod]
    public async Task FindAllUsers()
    {
        DatabaseInitializer.ResetDb();

        List<Person> users;

        using (var uow = new UnitOfWork())
        {
            users = await uow.FindAllByRole(Roles.USER);
        }

        Assert.IsNotNull(users);
        Assert.AreEqual(users.Count, 1);
        Assert.AreEqual(users[0].Username, "Filip");
    }

    [TestMethod]
    public async Task FindAllAdmins()
    {
        DatabaseInitializer.ResetDb();

        List<Person> admins;

        using (var uow = new UnitOfWork())
        {
            admins = await uow.FindAllByRole(Roles.ADMIN);
        }

        Assert.IsNotNull(admins);
        Assert.AreEqual(admins.Count, 1);
        Assert.AreEqual(admins[0].Username, "Admin");
    }

    [TestMethod]
    public async Task CreateUser()
    {
        DatabaseInitializer.ResetDb();
        var username = "Terka";
        var role = Roles.USER;
        string password = "This password is weak";

        Person person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.CreateUser(username, password, role);
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Username, username);
        Assert.AreEqual(person.Role, Roles.USER);
    }

    [TestMethod]
    public async Task CreateAdmin()
    {
        DatabaseInitializer.ResetDb();

        var username = "Terka";
        var role = Roles.ADMIN;
        string password = "This password is really strong";

        Person person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.CreateUser(username, password, role);
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Username, username);
        Assert.AreEqual(person.Role, role);
    }

    [TestMethod]
    public async Task UpdatePersonRoleToDifferent()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        using (var uow = new UnitOfWork())
        {
            await uow.UpdatePersonRole(person.Id, Roles.ADMIN);
        }

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Role, Roles.ADMIN);
    }

    [TestMethod]
    public async Task UpdatePersonRoleToSame()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        using (var uow = new UnitOfWork())
        {
            await uow.UpdatePersonRole(person.Id, Roles.USER);
        }

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Role, Roles.USER);
    }

    [TestMethod]
    public async Task UpdateUsername()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        using (var uow = new UnitOfWork())
        {
            await uow.UpdatePersonUsername(person.Id, "Jakub");
        }

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNull(person);

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Jakub");
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Username, "Jakub");
    }

    [TestMethod]
    public async Task GetPasswordHash()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        PasswordHash? hash;

        using (var uow = new UnitOfWork())
        {
            hash = await uow.getPasswordHash(person.Id);
        }

        Assert.IsNotNull(hash);
        Assert.AreEqual(hash.Hash, HelperFunctions.HashPassword("Filip"));
    }

    [TestMethod]
    public async Task GetPasswordHashNonExistUser()
    {
        DatabaseInitializer.ResetDb();

        int idNesmysl = 150;

        PasswordHash? hash;

        using (var uow = new UnitOfWork())
        {
            hash = await uow.getPasswordHash(idNesmysl);
        }

        Assert.IsNull(hash);
    }

    [TestMethod]
    public async Task ChangePassword()
    {
        DatabaseInitializer.ResetDb();
        string newPassword = "Silne Heslo";

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        using (var uow = new UnitOfWork())
        {
            await uow.ChangePassword(person.Id, newPassword);
        }

        PasswordHash? hashCode;
        using (var uow = new UnitOfWork())
        {
            hashCode = await uow.getPasswordHash(person.Id);
        }

        Assert.IsNotNull(hashCode);
        Assert.AreEqual(hashCode.Hash, HelperFunctions.HashPassword(newPassword));
    }

    [TestMethod]
    public async Task DeleteUser()
    {
        DatabaseInitializer.ResetDb();
        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        using (var uow = new UnitOfWork())
        {
            await uow.DeleteUser(person.Id);
        }

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNull(person);
    }

    [TestMethod]
    public async Task GetUserExpenses()
    {
        DatabaseInitializer.ResetDb();
        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        List<Expense> expenses;
        using (var uow = new UnitOfWork())
        {
            expenses = await uow.GetExpenses(person.Id);
        }

        Assert.AreEqual(expenses.Count, 3);
    }

    [TestMethod]
    public async Task AddExpense()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        using (var uow = new UnitOfWork())
        {
            await uow.AddExpense("Nova Kategorie", 300, person.Id);
        }

        List<Expense> expenses;
        using (var uow = new UnitOfWork())
        {
            expenses = await uow.GetExpenses(person.Id);
        }

        Assert.AreEqual(expenses.Count, 4);
    }

    [TestMethod]
    public async Task RemoveExpense()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        List<Expense> expenses;

        using (var uow = new UnitOfWork())
        {
            expenses = await uow.GetExpenses(person.Id);
        }

        Assert.IsNotNull(expenses);
        Assert.AreEqual(expenses.Count, 3);

        using (var uow = new UnitOfWork())
        {
            await uow.RemoveExpense(expenses[0].ObjectId);
        }

        using (var uow = new UnitOfWork())
        {
            expenses = await uow.GetExpenses(person.Id);
        }

        Assert.IsNotNull(expenses);
        Assert.AreEqual(expenses.Count, 2);
    }

    [TestMethod]
    public async Task GetExpensesByCategoryExist()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        List<Expense> expenses;
        using (var uow = new UnitOfWork())
        {
            expenses = await uow.GetExpensesByCategory(person.Id, "Beer");
        }

        Assert.IsNotNull(expenses);
        Assert.AreEqual(expenses.Count, 1);
        Assert.AreEqual(expenses[0].Category, "Beer");
    }

    public async Task GetExpensesByCategoryDontExist()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = await uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        List<Expense> expenses;
        using (var uow = new UnitOfWork())
        {
            expenses = await uow.GetExpensesByCategory(person.Id, "Neexistuje");
        }

        Assert.IsNotNull(expenses);
        Assert.AreEqual(expenses, new List<Expense>());
    }
}
