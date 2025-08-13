using Web_API_Assignment.Models;
using Web_API_Assignment.Repository;
using Web_API_Assignment.Services;

namespace Web_API_Assignment.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IActivityLogRepository _logRepository;

        public UserService(IUserRepository userRepository, IActivityLogRepository logRepository)
        {
            _userRepository = userRepository;
            _logRepository = logRepository;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User?> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<int> CreateUser(User user, int performedByUserId)
        {
            int newId = await _userRepository.Add(user);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "CREATE",
                TableAffected = "Users",
                RecordID = newId,
                Status = "Success"
            });

            return newId;
        }

        public async Task<bool> UpdateUser(User user, int performedByUserId)
        {
            bool updated = await _userRepository.Update(user);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "UPDATE",
                TableAffected = "Users",
                RecordID = user.UserID,
                Status = updated ? "Success" : "Failed"
            });

            return updated;
        }

        public async Task<bool> DeleteUser(int id, int performedByUserId)
        {
            bool deleted = await _userRepository.Delete(id);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "DELETE",
                TableAffected = "Users",
                RecordID = id,
                Status = deleted ? "Success" : "Failed"
            });

            return deleted;
        }
    }
}
