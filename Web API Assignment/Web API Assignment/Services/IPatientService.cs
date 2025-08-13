using Web_API_Assignment.Models;

namespace Web_API_Assignment.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAll();
        Task<Patient?> GetById(int id);
        Task<int> CreatePatient(Patient patient, int performedByUserId);
        Task<bool> UpdatePatient(Patient patient, int performedByUserId);
        Task<bool> DeletePatient(int id, int performedByUserId);
    }
}
