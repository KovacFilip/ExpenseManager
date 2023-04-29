using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB;
using DB.models;
using Microsoft.EntityFrameworkCore;

namespace DB.Repository
{
    public class PasswordHashRepository
    {
        private ExpensesDbContext _context;

        public PasswordHashRepository(ExpensesDbContext context)
        {
            _context = context;
        }

        DbSet<PasswordHash> passwordHashes => _context.PasswordHashes;

        public void AddUserPassword(PasswordHash passwordHash)
        {
            passwordHashes.Add(passwordHash);
        }

        public void DeleteUserPassword(int id)
        {
            var passwordHash = passwordHashes.Find(id);
            if (passwordHash != null)
            {
                passwordHashes.Remove(passwordHash);
            }
        }

        public PasswordHash? FindUserPasswordHash(int id)
        {
            return passwordHashes.Find(id);
        }

        public void UpdatePasswordHash(int id, string hash)
        {
            var passwordHash = passwordHashes.Find(id);
            if (passwordHash != null)
            {
                passwordHash.Hash = hash;
            }
        }
    }
}
