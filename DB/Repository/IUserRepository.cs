using DB.models;

namespace DB.Repository
{
    public interface IUserRepository
    {
        Task<Person> AddPerson(Person person);
        Task DeletePerson(int id);
        Task UpdatePersonRole(int id, Roles role);
        Task UpdatePersonUsername(int id, string username);
        Task<Person?> FindPersonById(int id);
        Task<Person?> FindPersonByUsername(string username);
        Task<List<Person>> FindAllByRole(Roles role);
    }
}
