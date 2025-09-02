using Web_API_Assignment.Models;

namespace Web_API_Assignment.Repository
{
    public interface IVisitRepository
    {
        Task<IEnumerable<Visit>> GetAll();
        Task<Visit?> GetById(int id);
        Task<int> Add(Visit visit);
        Task<bool> Update(Visit visit);
        Task<bool> Delete(int id);
    }
}
