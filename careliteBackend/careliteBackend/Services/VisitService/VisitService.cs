using careliteBackend.DTOs;
using careliteBackend.Repository.VisitNoteRepository;

namespace careliteBackend.Services.VisitService
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _repo;

        public VisitService(IVisitRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<VisitDto>> GetAllVisits()
        {
            return await _repo.GetVisits();
        }

        public async Task<VisitDto?> GetVisitById(int appointmentId)
        {
            return await _repo.GetVisitById(appointmentId);
        }
        public async Task<int> CreateVisit(CreateVisitRequest request)
        {
           return await _repo.AddVisit(request.AppointmentID, request.Content);
        }

        public async Task<int> UpdateVisit(int visitNoteId, UpdateVisitRequest request)
        {
            Console.WriteLine("Content in service: ", request.content);
            return await _repo.UpdateVisit(visitNoteId, request.content);
        }

        public async Task<int> DeleteVisit(int visitNoteId)
        {
            return await _repo.DeleteVisit(visitNoteId);
        }
    }
}
