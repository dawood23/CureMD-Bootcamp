using API_Demo.Repository;
using Web_API_Assignment.Models;
using Web_API_Assignment.Repository;
using Web_API_Assignment.Services;

namespace Web_API_Assignment.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _visitRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IVisitTypeRepository _visitTypeRepository;
        private readonly IActivityLogRepository _logRepository;

        public VisitService(
            IVisitRepository visitRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IVisitTypeRepository visitTypeRepository,
            IActivityLogRepository logRepository)
        {
            _visitRepository = visitRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _visitTypeRepository = visitTypeRepository;
            _logRepository = logRepository;
        }

        public async Task<IEnumerable<Visit>> GetAll()
        {
            return await _visitRepository.GetAll();
        }

        public async Task<Visit?> GetById(int id)
        {
            return await _visitRepository.GetById(id);
        }

        public async Task<int> CreateVisit(Visit visit, int performedByUserId)
        {
            if (await _patientRepository.GetById(visit.PatientID) == null)
                throw new Exception("Invalid PatientID");

            var visitType = await _visitTypeRepository.GetById(visit.VisitTypeID);
            if (visitType == null)
                throw new Exception("Invalid VisitTypeID");

            if (visit.DoctorID.HasValue && await _doctorRepository.GetById(visit.DoctorID.Value) == null)
                throw new Exception("Invalid DoctorID");

            if (string.IsNullOrWhiteSpace(visit.Status))
                visit.Status = "Scheduled";

            if (!visit.Fee.HasValue)
                visit.Fee = visitType.BaseFee;

            int newId = await _visitRepository.Add(visit);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "CREATE",
                TableAffected = "Visits",
                RecordID = newId,
                Status = "Success"
            });

            return newId;
        }

        public async Task<bool> UpdateVisit(Visit visit, int performedByUserId)
        {
            bool updated = await _visitRepository.Update(visit);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "UPDATE",
                TableAffected = "Visits",
                RecordID = visit.VisitID,
                Status = updated ? "Success" : "Failed"
            });

            return updated;
        }

        public async Task<bool> DeleteVisit(int id, int performedByUserId)
        {
            bool deleted = await _visitRepository.Delete(id);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "DELETE",
                TableAffected = "Visits",
                RecordID = id,
                Status = deleted ? "Success" : "Failed"
            });

            return deleted;
        }
    }
}
