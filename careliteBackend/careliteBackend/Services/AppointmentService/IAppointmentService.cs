using careliteBackend.DTOs;
using careliteBackend.Models;

namespace careliteBackend.Services.AppointmentService
{
    public interface IAppointmentService
    {
        Task<int> Create(AppointmentRequest request);
        Task<List<AppointmentDto>> GetAll();
        Task<AppointmentDto?> GetAppointmentByID(int id);
        Task<int> Update(AppointmentRequest request);
        Task<int> Delete(int id);

        Task<IEnumerable<WeeklyCalendarDto>> GetWeeklyCalendar(int doctorId, DateTime weekStartDate);
    }
}
