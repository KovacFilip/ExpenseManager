using DB.models;
using DB.Repository;
using Helper.Helpers;

namespace DB.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
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

        public async Task<Person> CreateUser(string username, string password, Roles role)
        {
            Person user = new Person { Username = username, Role = role };
            var person = await _userRepo.AddPerson(user);

            string hash = HelperFunctions.HashPassword(password);

            PasswordHash passwordHash = new PasswordHash { Hash = hash, PersonId = person.Id };
            await _passwordRepo.AddUserPassword(passwordHash);
            Commit();

            return user;
        }

        public async Task DeleteUser(int id)
        {
            await _userRepo.DeletePerson(id);
            await _passwordRepo.DeleteUserPassword(id);
            Commit();
        }

        public async Task UpdatePersonRole(int id, Roles role)
        {
            await _userRepo.UpdatePersonRole(id, role);
            Commit();
        }

        public async Task UpdatePersonUsername(int id, string username)
        {
            await _userRepo.UpdatePersonUsername(id, username);
            Commit();
        }

        public Task<Person?> FindPersonByUsername(string username)
        {
            return _userRepo.FindPersonByUsername(username);
        }

        public Task<List<Person>> FindAllByRole(Roles role)
        {
            return _userRepo.FindAllByRole(role);
        }

        public async Task ChangePassword(int id, string password)
        {
            var hash = HelperFunctions.HashPassword(password);
            await _passwordRepo.UpdatePasswordHash(id, hash);
            Commit();
        }

        public Task<PasswordHash?> getPasswordHash(int id)
        {
            return _passwordRepo.FindUserPasswordHash(id);
        }

        public async Task AddExpense(string category, int price, int personId)
        {
            var expense = new Expense
            {
                Category = category,
                Price = price,
                PersonId = personId
            };
            await _expenseRepo.AddExpense(expense);
            Commit();
        }

        public async Task RemoveExpense(int expenseId)
        {
            await _expenseRepo.RemoveExpense(expenseId);
            Commit();
        }

        public Task<List<Expense>> GetExpenses(int userId)
        {
            return _expenseRepo.GetAllUserExpenses(userId);
        }

        public async Task<List<Expense>> GetExpensesByCategory(int userId, string category)
        {
            var expenses = await _expenseRepo.GetAllUserExpenses(userId);

            return expenses.Where((expense) => expense.Category == category).ToList();
        }

        public async Task<Person?> Login(string username, string passwordHash)
        {
            var person = await _userRepo.FindPersonByUsername(username);

            if (person == null)
            {
                return null;
            }

            var personHash = await _passwordRepo.FindUserPasswordHash(person.Id);
            if (passwordHash == personHash!.Hash)
            {
                return person;
            }

            return null;
        }
    }
}
