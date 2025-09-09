using careliteBackend.DTOs;
using careliteBackend.Models;

namespace careliteBackend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> Authenticate(string username,string password);
        Task<int> CreateUser(User user);
    }
}
