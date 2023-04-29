using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB.models;
using Microsoft.EntityFrameworkCore;

namespace DB.Repository
{
    public class ExpenseRepository
    {
        private ExpensesDbContext _context;

        public ExpenseRepository(ExpensesDbContext context)
        {
            _context = context;
        }

        DbSet<Expense> expenses => _context.Expenses;

        public void AddExpense(Expense expense)
        {
            expenses.Add(expense);
        }

        public void RemoveExpense(int expenseId)
        {
            var expense = expenses.Find(expenseId);
            if (expense != null)
            {
                expenses.Remove(expense);
            }
        }

        public List<Expense> GetAllUserExpenses(int userId)
        {
            return expenses.Where(expense => expense.PersonId == userId).ToList();
        }
    }
}
