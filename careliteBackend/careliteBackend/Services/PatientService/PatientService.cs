using careliteBackend.Models;
using careliteBackend.Repository;

namespace careliteBackend.Services.PatientService
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;
        public PatientService(IPatientRepository repo) => _repo = repo;

        public Task<int> CreatePatient(Patient patient) => _repo.CreatePatient(patient);
        public Task<Patient?> GetPatientById(int patientId) => _repo.GetPatientById(patientId);
        public Task<List<Patient>> GetAllPatients() => _repo.GetAllPatients();
        public Task<int> UpdatePatient(Patient patient) => _repo.UpdatePatient(patient);
        public Task<int> DeletePatient(int patientId) => _repo.DeletePatient(patientId);
    }
}
