using Web_API_Assignment.Models;

namespace Web_API_Assignment.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(int id);
        Task<User?> GetByUsername(string username);
        Task<int> Add(User user);
        Task<bool> Update(User user);
        Task<bool> Delete(int id);
    }
}