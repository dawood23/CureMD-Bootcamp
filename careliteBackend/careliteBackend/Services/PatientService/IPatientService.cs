using careliteBackend.Models;

namespace careliteBackend.Services.PatientService
{
    public interface IPatientService
    {
        Task<int> CreatePatient(Patient patient);
        Task<Patient?> GetPatientById(int patientId);
        Task<List<Patient>> GetAllPatients();
        Task<int> UpdatePatient(Patient patient);
        Task<int> DeletePatient(int patientId);
    }
}
