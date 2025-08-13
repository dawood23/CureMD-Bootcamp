using Web_API_Assignment.Models;

namespace Web_API_Assignment.Services
{
    public interface IVisitService
    {
        Task<IEnumerable<Visit>> GetAll();
        Task<Visit?> GetById(int id);
        Task<int> CreateVisit(Visit visit, int performedByUserId);
        Task<bool> UpdateVisit(Visit visit, int performedByUserId);
        Task<bool> DeleteVisit(int id, int performedByUserId);
    }
}
