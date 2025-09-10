using careliteBackend.DTOs;

namespace careliteBackend.Repository.VisitNoteRepository
{
    public interface IVisitRepository
    {
        Task<IEnumerable<VisitDto>> GetVisits();
        Task<int> AddVisit(int appointmentId, string content);

        Task<VisitDto?> GetVisitById(int appointmentId);
        Task<int> UpdateVisit(int VisitNoteId,string content);
        Task<int> DeleteVisit(int visitNoteId);
    }
}
