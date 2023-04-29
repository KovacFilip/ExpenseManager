namespace Tests;

using DB.models;
using DB.UnitOfWork;
using DB.Utilities;

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
    public void Login()
    {
        DatabaseInitializer.ResetDb();
        Person? person;

        using (var uow = new UnitOfWork())
        {
            person = uow.Login("Filip", "Filip");
        }

        Assert.IsNotNull(person);
        Assert.AreEqual("Filip", person.Username);

        using (var uow = new UnitOfWork())
        {
            person = uow.Login("Filip", "Filip123");
        }

        Assert.IsNull(person);
    }
}
