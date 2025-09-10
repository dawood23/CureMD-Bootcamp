using careliteBackend.DTOs;

namespace careliteBackend.Services.VisitService
{
    public interface IVisitService
    {
        Task<IEnumerable<VisitDto>> GetAllVisits();
        Task<VisitDto?> GetVisitById(int appointmentId);
        Task<int> CreateVisit(CreateVisitRequest request);
        Task<int> UpdateVisit(int visitNoteId, UpdateVisitRequest request);
        Task<int> DeleteVisit(int visitNoteId);
    }
}
