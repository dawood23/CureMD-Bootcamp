using careliteBackend.Models;

namespace careliteBackend.Services.NotificationService
{
    public interface INotificationService
    {
        Task? AppointmentNotification(int appointmentID);

        Task? TransactionNotification(int TransactionID);
      
    }
}
