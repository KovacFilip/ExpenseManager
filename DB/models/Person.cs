using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB.models
{
    public class Person
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Role { get; set; }
        public List<Expense> Expenses { get; set; }
        public PasswordHash PasswordHash { get; set; }
    }
}
