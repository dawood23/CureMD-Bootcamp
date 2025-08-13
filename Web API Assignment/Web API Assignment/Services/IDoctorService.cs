using Web_API_Assignment.Models;

namespace Web_API_Assignment.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<Doctor>> GetAll();
        Task<Doctor?> GetById(int id);
        Task<int> CreateDoctor(Doctor doctor, int performedByUserId);
        Task<bool> UpdateDoctor(Doctor doctor, int performedByUserId);
        Task<bool> DeleteDoctor(int id, int performedByUserId);
    }
}


