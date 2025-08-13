using Web_API_Assignment.Models;
using Web_API_Assignment.Repository;

namespace Web_API_Assignment.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IActivityLogRepository _logRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository, IActivityLogRepository logRepository)
        {
            _userRoleRepository = userRoleRepository;
            _logRepository = logRepository;
        }

        public async Task<IEnumerable<UserRole>> GetAll()
        {
            return await _userRoleRepository.GetAll();
        }

        public async Task<UserRole?> GetById(int id)
        {
            return await _userRoleRepository.GetById(id);
        }

        public async Task<int> CreateUserRole(UserRole userRole, int performedByUserId)
        {
            int newId = await _userRoleRepository.Add(userRole);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "CREATE",
                TableAffected = "UserRoles",
                RecordID = newId,
                Status = "Success"
            });

            return newId;
        }

        public async Task<bool> UpdateUserRole(UserRole userRole, int performedByUserId)
        {
            bool updated = await _userRoleRepository.Update(userRole);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "UPDATE",
                TableAffected = "UserRoles",
                RecordID = userRole.RoleID,
                Status = updated ? "Success" : "Failed"
            });

            return updated;
        }

        public async Task<bool> DeleteUserRole(int id, int performedByUserId)
        {
            bool deleted = await _userRoleRepository.Delete(id);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "DELETE",
                TableAffected = "UserRoles",
                RecordID = id,
                Status = deleted ? "Success" : "Failed"
            });

            return deleted;
        }
    }
}
