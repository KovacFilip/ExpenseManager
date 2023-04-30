using DB.models;
using Microsoft.EntityFrameworkCore;

namespace DB.Repository
{
    public class UserRepository
    {
        private ExpensesDbContext _context;

        public UserRepository(ExpensesDbContext context)
        {
            _context = context;
        }

        private DbSet<Person> people => _context.People;

        public async Task<Person> AddPerson(Person person)
        {
            await people.AddAsync(person);
            return person;
        }

        public async Task DeletePerson(int id)
        {
            var person = await people.FindAsync(id);
            if (person != null)
            {
                people.Remove(person);
            }
        }

        public async Task UpdatePersonRole(int id, Roles role)
        {
            var person = await people.FindAsync(id);
            if (person != null)
            {
                person.Role = role;
            }
        }

        public async Task UpdatePersonUsername(int id, string username)
        {
            var person = await people.FindAsync(id);
            if (person != null)
            {
                person.Username = username;
            }
        }

        public async Task<Person?> FindPersonById(int id)
        {
            return await people.FindAsync(id);
        }

        public Task<Person?> FindPersonByUsername(string username)
        {
            return Task.Run(() =>
            {
                foreach (var person in people)
                {
                    if (person.Username == username)
                    {
                        return person;
                    }
                }

                return null;
            });
        }

        public Task<List<Person>> FindAllByRole(Roles role)
        {
            return Task.Run(() =>
            {
                return people.Where(person => person.Role == role).ToList();
            });
        }
    }
}
