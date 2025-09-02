using Web_API_Assignment.Models;

namespace Web_API_Assignment.Services
{
    public interface IVisitTypeService
    {
        Task<IEnumerable<VisitType>> GetAll();
        Task<VisitType?> GetById(int id);
        Task<int> CreateVisitType(VisitType visitType, int performedByUserId);
        Task<bool> UpdateVisitType(VisitType visitType, int performedByUserId);
        Task<bool> DeleteVisitType(int id, int performedByUserId);
    }
}
