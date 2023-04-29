namespace Tests;

using DB.models;
using DB.UnitOfWork;
using DB.Utilities;
using Helper.Helpers;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void DatabaseReset()
    {
        DatabaseInitializer.ResetDb();
        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Admin");
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Role, Roles.ADMIN);
        Assert.IsNotNull(person.Id);

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Role, Roles.USER);
        Assert.IsNotNull(person.Id);

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Neexistuje");
        }

        Assert.IsNull(person);
    }

    [TestMethod]
    public void LoginCorrect()
    {
        DatabaseInitializer.ResetDb();
        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.Login("Filip", "Filip");
        }

        Assert.IsNotNull(person);
        Assert.AreEqual("Filip", person.Username);
    }

    [TestMethod]
    public void LoginIncorrectPassword()
    {
        DatabaseInitializer.ResetDb();

        Person? person;
        using (var uow = new UnitOfWork())
        {
            person = uow.Login("Filip", "TohleNeniMojeHeslo");
        }

        Assert.IsNull(person);
    }

    [TestMethod]
    public void LoginIncorrectUsername()
    {
        DatabaseInitializer.ResetDb();

        Person? person;
        using (var uow = new UnitOfWork())
        {
            person = uow.Login("Blbost", "Neexistuje");
        }

        Assert.IsNull(person);
    }

    [TestMethod]
    public void FindPersonByNameExists()
    {
        DatabaseInitializer.ResetDb();

        Person? person;
        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);
    }

    [TestMethod]
    public void FindPersonByNameNotExists()
    {
        DatabaseInitializer.ResetDb();

        Person? person;
        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Výmysl");
        }

        Assert.IsNull(person);
    }

    [TestMethod]
    public void FindAllUsers()
    {
        DatabaseInitializer.ResetDb();

        List<Person> users;

        using (var uow = new UnitOfWork())
        {
            users = uow.FindAllByRole(Roles.USER);
        }

        Assert.IsNotNull(users);
        Assert.AreEqual(users.Count, 1);
        Assert.AreEqual(users[0].Username, "Filip");
    }

    [TestMethod]
    public void FindAllAdmins()
    {
        DatabaseInitializer.ResetDb();

        List<Person> admins;

        using (var uow = new UnitOfWork())
        {
            admins = uow.FindAllByRole(Roles.ADMIN);
        }

        Assert.IsNotNull(admins);
        Assert.AreEqual(admins.Count, 1);
        Assert.AreEqual(admins[0].Username, "Admin");
    }

    [TestMethod]
    public void CreateUser()
    {
        DatabaseInitializer.ResetDb();
        var username = "Terka";
        var role = Roles.USER;
        string password = "This password is weak";

        Person person;

        using (var uow = new UnitOfWork())
        {
            person = uow.CreateUser(username, password, role);
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Username, username);
        Assert.AreEqual(person.Role, Roles.USER);
    }

    public void CreateAdmin()
    {
        DatabaseInitializer.ResetDb();

        var username = "Terka";
        var role = Roles.ADMIN;
        string password = "This password is really strong";

        Person person;

        using (var uow = new UnitOfWork())
        {
            person = uow.CreateUser(username, password, role);
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Username, username);
        Assert.AreEqual(person.Role, role);
    }

    [TestMethod]
    public void UpdatePersonRoleToDifferent()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        using (var uow = new UnitOfWork())
        {
            uow.UpdatePersonRole(person.Id, Roles.ADMIN);
        }

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Role, Roles.ADMIN);
    }

    [TestMethod]
    public void UpdatePersonRoleToSame()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        using (var uow = new UnitOfWork())
        {
            uow.UpdatePersonRole(person.Id, Roles.USER);
        }

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Role, Roles.USER);
    }

    [TestMethod]
    public void UpdateUsername()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        using (var uow = new UnitOfWork())
        {
            uow.UpdatePersonUsername(person.Id, "Jakub");
        }

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNull(person);

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Jakub");
        }

        Assert.IsNotNull(person);
        Assert.AreEqual(person.Username, "Jakub");
    }

    [TestMethod]
    public void GetPasswordHash()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        PasswordHash? hash;

        using (var uow = new UnitOfWork())
        {
            hash = uow.getPasswordHash(person.Id);
        }

        Assert.IsNotNull(hash);
        Assert.AreEqual(hash.Hash, HelperFunctions.HashPassword("Filip"));
    }

    [TestMethod]
    public void GetPasswordHashNonExistUser()
    {
        DatabaseInitializer.ResetDb();

        int idNesmysl = 150;

        PasswordHash? hash;

        using (var uow = new UnitOfWork())
        {
            hash = uow.getPasswordHash(idNesmysl);
        }

        Assert.IsNull(hash);
    }

    [TestMethod]
    public void ChangePassword()
    {
        DatabaseInitializer.ResetDb();
        string newPassword = "Silne Heslo";

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        using (var uow = new UnitOfWork())
        {
            uow.ChangePassword(person.Id, newPassword);
        }

        PasswordHash? hashCode;
        using (var uow = new UnitOfWork())
        {
            hashCode = uow.getPasswordHash(person.Id);
        }

        Assert.IsNotNull(hashCode);
        Assert.AreEqual(hashCode.Hash, HelperFunctions.HashPassword(newPassword));
    }

    [TestMethod]
    public void DeleteUser()
    {
        DatabaseInitializer.ResetDb();
        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        using (var uow = new UnitOfWork())
        {
            uow.DeleteUser(person.Id);
        }

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNull(person);
    }

    [TestMethod]
    public void GetUserExpenses()
    {
        DatabaseInitializer.ResetDb();
        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        List<Expense> expenses;
        using (var uow = new UnitOfWork())
        {
            expenses = uow.GetExpenses(person.Id);
        }

        Assert.AreEqual(expenses.Count, 3);
    }

    [TestMethod]
    public void AddExpense()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        using (var uow = new UnitOfWork())
        {
            uow.AddExpense("Nova Kategorie", 300, person.Id);
        }

        List<Expense> expenses;
        using (var uow = new UnitOfWork())
        {
            expenses = uow.GetExpenses(person.Id);
        }

        Assert.AreEqual(expenses.Count, 4);
    }

    [TestMethod]
    public void RemoveExpense()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        List<Expense> expenses;

        using (var uow = new UnitOfWork())
        {
            expenses = uow.GetExpenses(person.Id);
        }

        Assert.IsNotNull(expenses);
        Assert.AreEqual(expenses.Count, 3);

        using (var uow = new UnitOfWork())
        {
            uow.RemoveExpense(expenses[0].ObjectId);
        }

        using (var uow = new UnitOfWork())
        {
            expenses = uow.GetExpenses(person.Id);
        }

        Assert.IsNotNull(expenses);
        Assert.AreEqual(expenses.Count, 2);
    }

    [TestMethod]
    public void GetExpensesByCategoryExist()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        List<Expense> expenses;
        using (var uow = new UnitOfWork())
        {
            expenses = uow.GetExpensesByCategory(person.Id, "Beer");
        }

        Assert.IsNotNull(expenses);
        Assert.AreEqual(expenses.Count, 1);
        Assert.AreEqual(expenses[0].Category, "Beer");
    }

    public void GetExpensesByCategoryDontExist()
    {
        DatabaseInitializer.ResetDb();

        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.FindPersonByUsername("Filip");
        }

        Assert.IsNotNull(person);

        List<Expense> expenses;
        using (var uow = new UnitOfWork())
        {
            expenses = uow.GetExpensesByCategory(person.Id, "Neexistuje");
        }

        Assert.IsNotNull(expenses);
        Assert.AreEqual(expenses, new List<Expense>());
    }
}
