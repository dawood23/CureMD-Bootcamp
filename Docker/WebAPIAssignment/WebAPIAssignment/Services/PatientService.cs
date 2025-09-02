using Web_API_Assignment.Models;
using Web_API_Assignment.Repository;
using Web_API_Assignment.Services;

namespace Web_API_Assignment.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IActivityLogRepository _logRepository;

        public PatientService(IPatientRepository patientRepository, IActivityLogRepository logRepository)
        {
            _patientRepository = patientRepository;
            _logRepository = logRepository;
        }

        public async Task<IEnumerable<Patient>> GetAll()
        {
            return await _patientRepository.GetAll();
        }

        public async Task<Patient?> GetById(int id)
        {
            return await _patientRepository.GetById(id);
        }

        public async Task<int> CreatePatient(Patient patient, int performedByUserId)
        {
            int newId = await _patientRepository.Add(patient);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "CREATE",
                TableAffected = "Patients",
                RecordID = newId,
                Status = "Success"
            });

            return newId;
        }

        public async Task<bool> UpdatePatient(Patient patient, int performedByUserId)
        {
            bool updated = await _patientRepository.Update(patient);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "UPDATE",
                TableAffected = "Patients",
                RecordID = patient.PatientID,
                Status = updated ? "Success" : "Failed"
            });

            return updated;
        }

        public async Task<bool> DeletePatient(int id, int performedByUserId)
        {
            bool deleted = await _patientRepository.Delete(id);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "DELETE",
                TableAffected = "Patients",
                RecordID = id,
                Status = deleted ? "Success" : "Failed"
            });

            return deleted;
        }
    }
}
