using DB.models;
using Microsoft.EntityFrameworkCore;

namespace DB.Repository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private ExpensesDbContext _context;

        public ExpenseRepository(ExpensesDbContext context)
        {
            _context = context;
        }

        DbSet<Expense> expenses => _context.Expenses;

        public async Task AddExpense(Expense expense)
        {
            await expenses.AddAsync(expense);
        }

        public async Task RemoveExpense(int expenseId)
        {
            var expense = await expenses.FindAsync(expenseId);
            if (expense != null)
            {
                expenses.Remove(expense);
            }
        }

        public Task<List<Expense>> GetAllUserExpenses(int userId)
        {
            return Task.Run(() =>
            {
                return expenses.Where(expense => expense.PersonId == userId).ToList();
            });
        }
    }
}
