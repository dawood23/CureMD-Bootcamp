using Web_API_Assignment.Models;

namespace API_Demo.Repository
{
    public interface IVisitTypeRepository
    {
        Task<IEnumerable<VisitType>> GetAll();
        Task<VisitType?> GetById(int id);
        Task<int> Add(VisitType type);
        Task<bool> Update(VisitType type);
        Task<bool> Delete(int id);
    }
}
