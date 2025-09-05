using careliteBackend.Models;

namespace careliteBackend.Services.Interfaces
{
    public interface IRoleService
    {
        Task<int> CreateRole(Role role);
        Task<Role?> GetRoleById(int roleId);
        Task<List<Role>> GetAllRoles();
        Task<int> UpdateRole(Role role);
        Task<int> DeleteRole(int roleId);
    }
}
