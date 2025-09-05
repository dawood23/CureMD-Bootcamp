using careliteBackend.Models;

namespace careliteBackend.Repository
{
    public interface IPatientRepository
    {
        Task<int> CreatePatient(Patient patient);
        Task<Patient?> GetPatientById(int patientId);
        Task<List<Patient>> GetAllPatients();
        Task<int> UpdatePatient(Patient patient);
        Task<int> DeletePatient(int patientId);
    }

}
