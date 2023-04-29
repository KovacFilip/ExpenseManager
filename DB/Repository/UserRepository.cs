using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public Person AddPerson(Person person)
        {
            people.Add(person);
            return person;
        }

        public void DeletePerson(int id)
        {
            var person = people.Find(id);
            if (person != null)
            {
                people.Remove(person);
            }
        }

        public void UpdatePersonRole(int id, Roles role)
        {
            var person = people.Find(id);
            if (person != null)
            {
                person.Role = role;
            }
        }

        public void UpdatePersonUsername(int id, string username)
        {
            var person = people.Find(id);
            if (person != null)
            {
                person.Username = username;
            }
        }

        public Person? FindPersonById(int id)
        {
            return people.Find(id);
        }

        public Person? FindPersonByUsername(string username)
        {
            foreach (var person in people)
            {
                if (person.Username == username)
                {
                    return person;
                }
            }

            return null;
        }

        public List<Person> FindAllByRole(Roles role)
        {
            return people.Where(person => person.Role == role).ToList();
        }
    }
}
