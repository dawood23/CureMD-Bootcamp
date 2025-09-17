using careliteBackend.Models;
using careliteBackend.DTOs;
namespace careliteBackend.Repository
{
    public interface IAppointmentRepository
    {
        Task<int> AddAppointment(AppointmentRequest request);
        Task<List<AppointmentDto>> GetAppointments();
        Task<AppointmentDto?> GetAppointmentByID(int id);
        Task<int> UpdateAppointment(UpdateAppointmentDto request);
        Task<int> DeleteAppointment(int id);

        Task<IEnumerable<WeeklyCalendarDto>> GetWeeklyCalendar(int doctorId, DateTime weekStartDate);
    }
}