using DB.models;

namespace DB.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<Person> CreateUser(string username, string password, Roles role);
        Task DeleteUser(int id);
        Task UpdatePersonRole(int id, Roles role);
        Task UpdatePersonUsername(int id, string username);
        Task<Person?> FindPersonByUsername(string username);
        Task<List<Person>> FindAllByRole(Roles role);
        Task ChangePassword(int id, string password);
        Task<PasswordHash?> getPasswordHash(int id);
        Task AddExpense(string category, int price, int personId);
        Task RemoveExpense(int expenseId);
        Task<List<Expense>> GetExpenses(int userId);
        Task<List<Expense>> GetExpensesByCategory(int userId, string category);
        Task<Person?> Login(string username, string password);
        void Commit();
    }
}
