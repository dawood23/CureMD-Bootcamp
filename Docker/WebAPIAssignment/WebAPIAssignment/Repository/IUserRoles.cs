using Web_API_Assignment.Models;

namespace Web_API_Assignment.Repository
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<UserRole>> GetAll();
        Task<UserRole?> GetById(int id);
        Task<int> Add(UserRole role);
        Task<bool> Update(UserRole role);
        Task<bool> Delete(int id);
    }
}
