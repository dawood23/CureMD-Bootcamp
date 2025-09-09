using careliteBackend.DTOs;
using careliteBackend.Models;

namespace careliteBackend.Services.AppointmentService
{
    public interface IAppointmentService
    {
        Task<int> Create(AppointmentRequest request);
        Task<List<AppointmentDto>> GetAll();
        Task<int> Update(AppointmentRequest request);
        Task<int> Delete(int id);
    }
}
