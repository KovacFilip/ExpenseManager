using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB.models;
using DB.Repository;
using Helper.Helpers;

namespace DB.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private ExpensesDbContext _context;
        private UserRepository _userRepo;
        private PasswordHashRepository _passwordRepo;
        private ExpenseRepository _expenseRepo;

        public UnitOfWork()
        {
            _context = new ExpensesDbContext();
            _userRepo = new UserRepository(_context);
            _passwordRepo = new PasswordHashRepository(_context);
            _expenseRepo = new ExpenseRepository(_context);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void CreateUser(string username, string password, Roles role)
        {
            Person user = new Person { Username = username, Role = role };
            var person = _userRepo.AddPerson(user);

            string hash = HelperFunctions.HashPassword(password);

            PasswordHash passwordHash = new PasswordHash { Hash = hash, PersonId = person.Id };
            _passwordRepo.AddUserPassword(passwordHash);
            Commit();
        }

        public void DeleteUser(int id)
        {
            _userRepo.DeletePerson(id);
            _passwordRepo.DeleteUserPassword(id);
            Commit();
        }

        public void UpdatePersonRole(int id, Roles role)
        {
            _userRepo.UpdatePersonRole(id, role);
            Commit();
        }

        public void UpdatePersonUsername(int id, string username)
        {
            _userRepo.UpdatePersonUsername(id, username);
            Commit();
        }

        public Person? FindPersonByUsername(string username)
        {
            return _userRepo.FindPersonByUsername(username);
        }

        public List<Person> FindAllByRole(Roles role)
        {
            return _userRepo.FindAllByRole(role);
        }

        public void ChangePassword(int id, string password)
        {
            var hash = HelperFunctions.HashPassword(password);
            _passwordRepo.UpdatePasswordHash(id, hash);
            Commit();
        }

        public PasswordHash? getPasswordHash(int id)
        {
            return _passwordRepo.FindUserPasswordHash(id);
        }

        public void AddExpense(string category, int price, int personId)
        {
            var expense = new Expense
            {
                Category = category,
                Price = price,
                PersonId = personId
            };
            _expenseRepo.AddExpense(expense);
            Commit();
        }

        public void RemoveExpense(int expenseId)
        {
            _expenseRepo.RemoveExpense(expenseId);
            Commit();
        }

        public List<Expense> GetExpenses(int userId)
        {
            return _expenseRepo.GetAllUserExpenses(userId);
        }

        public List<Expense> GetExpensesByCategory(int userId, string category)
        {
            return _expenseRepo
                .GetAllUserExpenses(userId)
                .Where((expense) => expense.Category == category)
                .ToList();
        }
    }
}
