using Web_API_Assignment.Models;

namespace Web_API_Assignment.Services
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRole>> GetAll();
        Task<UserRole?> GetById(int id);
        Task<int> CreateUserRole(UserRole userRole, int performedByUserId);
        Task<bool> UpdateUserRole(UserRole userRole, int performedByUserId);
        Task<bool> DeleteUserRole(int id, int performedByUserId);
    }
}
