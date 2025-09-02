using Web_API_Assignment.Models;

namespace Web_API_Assignment.Repository
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAll();
        Task<Doctor?> GetById(int id);
        Task<int> Add(Doctor doctor);
        Task<bool> Update(Doctor doctor);
        Task<bool> Delete(int id);
    }

}
