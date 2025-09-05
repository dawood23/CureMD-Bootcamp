using careliteBackend.DTOs;
using careliteBackend.Models;

namespace careliteBackend.Services.Interfaces
{
    public interface IAuth
    {
        Task<AuthResult> Authenticate(string username,string password);
        Task<int> CreateUser(User user);
    }
}
