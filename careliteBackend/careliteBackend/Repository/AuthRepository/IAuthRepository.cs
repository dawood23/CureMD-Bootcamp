using careliteBackend.Models;

namespace careliteBackend.Repository
{
    public interface IAuthRepository
    {
        Task<User?> GetByUsername(string username);
        Task<int?> CreateUser(User user); 
    }
}
