using careliteBackend.Models;

namespace careliteBackend.Repository
{
    public interface IRoleRepository
    {
        Task<int> CreateRole(Role role);
        Task<Role?> GetRoleById(int roleId);
        Task<List<Role>> GetAllRoles();
        Task<int> UpdateRole(Role role);
        Task<int> DeleteRole(int roleId);
    }
}
