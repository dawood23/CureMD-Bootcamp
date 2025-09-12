using careliteBackend.DTOs;
using careliteBackend.Models;
using careliteBackend.Repository;

namespace careliteBackend.Services.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> Create(AppointmentRequest request)
        {
            return await _repo.AddAppointment(request);
        }

        public async Task<List<AppointmentDto>> GetAll()
        {
            return await _repo.GetAppointments();
        }

        public async Task<AppointmentDto?> GetAppointmentByID(int id)
        {
            return await _repo.GetAppointmentByID(id);
        }

        public async Task<int> Update(AppointmentRequest request)
        {
            return await _repo.UpdateAppointment(request);
        }

        public async Task<int> Delete(int id)
        {
            return await _repo.DeleteAppointment(id);
        }

        public async Task<IEnumerable<WeeklyCalendarDto>> GetWeeklyCalendar(int doctorId, DateTime weekStartDate)
        {
            return await _repo.GetWeeklyCalendar(doctorId, weekStartDate);
        }
    }
}
