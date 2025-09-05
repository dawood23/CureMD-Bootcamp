using careliteBackend.Models;
using careliteBackend.Repository;
using careliteBackend.Services.Interfaces;

namespace careliteBackend.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository) => _roleRepository = roleRepository;

        public Task<int> CreateRole(Role role) => _roleRepository.CreateRole(role);
        public Task<Role?> GetRoleById(int roleId) => _roleRepository.GetRoleById(roleId);
        public Task<List<Role>> GetAllRoles() => _roleRepository.GetAllRoles();
        public Task<int> UpdateRole(Role role) => _roleRepository.UpdateRole(role);
        public Task<int> DeleteRole(int roleId) => _roleRepository.DeleteRole(roleId);
    }
}
