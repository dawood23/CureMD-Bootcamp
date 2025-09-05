using careliteBackend.Models;
using careliteBackend.Repository;

namespace careliteBackend.Services.DoctorService
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repo;
        public DoctorService(IDoctorRepository repo) => _repo = repo;

        public Task<int> CreateDoctor(Doctor doctor) => _repo.CreateDoctor(doctor);
        public Task<Doctor?> GetDoctorById(int doctorId) => _repo.GetDoctorById(doctorId);
        public Task<List<Doctor>> GetAllDoctors() => _repo.GetAllDoctors();
        public Task<int> UpdateDoctor(Doctor doctor) => _repo.UpdateDoctor(doctor);
        public Task<int> DeleteDoctor(int doctorId) => _repo.DeleteDoctor(doctorId);
    }
}
