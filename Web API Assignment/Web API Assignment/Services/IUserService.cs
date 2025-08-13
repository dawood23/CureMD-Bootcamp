using Web_API_Assignment.Models;

namespace Web_API_Assignment.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(int id);
        Task<int> CreateUser(User user, int performedByUserId);
        Task<bool> UpdateUser(User user, int performedByUserId);
        Task<bool> DeleteUser(int id, int performedByUserId);
    }
}
