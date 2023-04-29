using DB;
using DB.models;
using DB.Repository;
using DB.UnitOfWork;
using DB.Utilities;

DatabaseInitializer.ResetDb();

using (var unitOfWork = new UnitOfWork())
{
    unitOfWork.CreateUser("Terka", "abc123", Roles.USER);
}

for (int i = 1; i < 4; i++)
{
    using (var uow = new UnitOfWork())
    {
        Console.WriteLine($"{i} - {uow.getPasswordHash(i)!.Hash}");
    }
}
