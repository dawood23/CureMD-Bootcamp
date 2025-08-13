using API_Demo.Repository;
using Web_API_Assignment.Models;
using Web_API_Assignment.Repository;
using Web_API_Assignment.Services;

namespace Web_API_Assignment.Services
{
    public class VisitTypeService : IVisitTypeService
    {
        private readonly IVisitTypeRepository _visitTypeRepository;
        private readonly IActivityLogRepository _logRepository;

        public VisitTypeService(IVisitTypeRepository visitTypeRepository, IActivityLogRepository logRepository)
        {
            _visitTypeRepository = visitTypeRepository;
            _logRepository = logRepository;
        }

        public async Task<IEnumerable<VisitType>> GetAll()
        {
            return await _visitTypeRepository.GetAll();
        }

        public async Task<VisitType?> GetById(int id)
        {
            return await _visitTypeRepository.GetById(id);
        }

        public async Task<int> CreateVisitType(VisitType visitType, int performedByUserId)
        {
            if (visitType.BaseFee < 0)
                throw new Exception("Base fee must be non-negative");

            int newId = await _visitTypeRepository.Add(visitType);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "CREATE",
                TableAffected = "VisitTypes",
                RecordID = newId,
                Status = "Success"
            });

            return newId;
        }

        public async Task<bool> UpdateVisitType(VisitType visitType, int performedByUserId)
        {
            if (visitType.BaseFee < 0)
                throw new Exception("Base fee must be non-negative");

            bool updated = await _visitTypeRepository.Update(visitType);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "UPDATE",
                TableAffected = "VisitTypes",
                RecordID = visitType.VisitTypeID,
                Status = updated ? "Success" : "Failed"
            });

            return updated;
        }

        public async Task<bool> DeleteVisitType(int id, int performedByUserId)
        {
            bool deleted = await _visitTypeRepository.Delete(id);

            await _logRepository.Add(new ActivityLog
            {
                UserID = performedByUserId,
                Action = "DELETE",
                TableAffected = "VisitTypes",
                RecordID = id,
                Status = deleted ? "Success" : "Failed"
            });

            return deleted;
        }
    }
}

