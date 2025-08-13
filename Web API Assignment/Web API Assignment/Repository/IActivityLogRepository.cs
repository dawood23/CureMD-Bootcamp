using Web_API_Assignment.Models;

namespace Web_API_Assignment.Repository
{
    public interface IActivityLogRepository
    {
        Task<IEnumerable<ActivityLog>> GetAll();
        Task<ActivityLog?> GetById(int id);
        Task<int> Add(ActivityLog log);
        Task<bool> Delete(int id);
    }
}
