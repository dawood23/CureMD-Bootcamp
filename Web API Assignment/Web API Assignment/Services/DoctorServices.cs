using Web_API_Assignment.Models;
using Web_API_Assignment.Repository;
using Web_API_Assignment.Services;

namespace Web_API_Assignment.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IActivityLogRepository _logRepository;

        public DoctorService(IDoctorRepository doctorRepository, IActivityLogRepository logRepository)
        {
            _doctorRepository = doctorRepository;
            _logRepository = logRepository;
        }

        public async Task<IEnumerable<Doctor>> GetAll()
        {
            return await _doctorRepository.GetAll();
        }

        public async Task<Doctor?> GetById(int id)
        {
            return await _doctorRepository.GetById(id);
        }

        public async Task<int> CreateDoctor(Doctor doctor, int performedByUserId)
        {
            int newId = await _doctorRepository.Add(doctor);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "CREATE",
                TableAffected = "Doctors",
                RecordID = newId,
                Status = "Success"
            });

            return newId;
        }

        public async Task<bool> UpdateDoctor(Doctor doctor, int performedByUserId)
        {
            bool updated = await _doctorRepository.Update(doctor);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "UPDATE",
                TableAffected = "Doctors",
                RecordID = doctor.DoctorID,
                Status = updated ? "Success" : "Failed"
            });

            return updated;
        }

        public async Task<bool> DeleteDoctor(int id, int performedByUserId)
        {
            bool deleted = await _doctorRepository.Delete(id);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "DELETE",
                TableAffected = "Doctors",
                RecordID = id,
                Status = deleted ? "Success" : "Failed"
            });

            return deleted;
        }
    }
}

