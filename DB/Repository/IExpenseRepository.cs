using DB.models;

namespace DB.Repository
{
    public interface IExpenseRepository
    {
        Task AddExpense(Expense expense);
        Task RemoveExpense(int expenseId);
        Task<List<Expense>> GetAllUserExpenses(int userId);
    }
}
