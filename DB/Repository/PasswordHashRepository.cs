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

        public async Task AddUserPassword(PasswordHash passwordHash)
        {
            await passwordHashes.AddAsync(passwordHash);
        }

        public async Task DeleteUserPassword(int id)
        {
            var passwordHash = await passwordHashes.FindAsync(id);
            if (passwordHash != null)
            {
                passwordHashes.Remove(passwordHash);
            }
        }

        public async Task<PasswordHash?> FindUserPasswordHash(int id)
        {
            return await passwordHashes.FindAsync(id);
        }

        public async Task UpdatePasswordHash(int id, string hash)
        {
            var passwordHash = await passwordHashes.FindAsync(id);
            if (passwordHash != null)
            {
                passwordHash.Hash = hash;
            }
        }
    }
}
