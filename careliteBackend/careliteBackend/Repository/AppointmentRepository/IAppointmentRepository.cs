using careliteBackend.Models;
using careliteBackend.DTOs;
namespace careliteBackend.Repository
{
    public interface IAppointmentRepository
    {
        Task<int> AddAppointment(AppointmentRequest request);
        Task<List<AppointmentDto>> GetAppointments();
        Task<int> UpdateAppointment(AppointmentRequest request);
        Task<int> DeleteAppointment(int id);
    }
}