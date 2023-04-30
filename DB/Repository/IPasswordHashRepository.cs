using DB.models;

namespace DB.Repository
{
    public interface IPasswordHashRepository
    {
        Task AddUserPassword(PasswordHash passwordHash);
        Task DeleteUserPassword(int id);
        Task<PasswordHash?> FindUserPasswordHash(int id);
        Task UpdatePasswordHash(int id, string hash);
    }
}
