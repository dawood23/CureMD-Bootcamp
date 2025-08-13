using Web_API_Assignment.Models;

namespace Web_API_Assignment.Repository
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAll();
        Task<Patient?> GetById(int id);
        Task<int> Add(Patient patient);
        Task<bool> Update(Patient patient);
        Task<bool> Delete(int id);
    }
}
