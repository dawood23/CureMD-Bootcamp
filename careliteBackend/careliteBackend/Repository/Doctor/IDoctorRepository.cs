using careliteBackend.Models;

namespace careliteBackend.Repository
{
    public interface IDoctorRepository
    {
        Task<int> CreateDoctor(Doctor doctor);
        Task<Doctor?> GetDoctorById(int doctorId);
        Task<List<Doctor>> GetAllDoctors();
        Task<int> UpdateDoctor(Doctor doctor);
        Task<int> DeleteDoctor(int doctorId);
    }
}
