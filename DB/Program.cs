using DB;
using DB.Utilities;

DatabaseInitializer.ResetDb();

using (var db = new ExpensesDbContext())
{
    var userPasswordHashExpenses = db.People.Join(
        db.PasswordHashes,
        person => person.Id,
        hash => hash.PersonId,
        (person, hash) => new { person, hash }
    );

    foreach (var user in userPasswordHashExpenses)
    {
        Console.WriteLine($"{user.person.Id}: {user.person.Username} - {user.hash.Hash}");
    }
}
