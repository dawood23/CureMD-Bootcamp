using careliteBackend.DTOs;
using careliteBackend.Repository;
using careliteBackend.Services.AppointmentService;
using careliteBackend.Services.BillService;
using careliteBackend.Services.PatientService;
using System.Net.Mail;

namespace careliteBackend.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IBillService _billService;

        private readonly string senderAddress = "dawood.nadeem@curemd.com"; 
        private readonly string senderName = "CareLite";

        public NotificationService(IAppointmentService appointmentService, IPatientService patientService,IBillService billService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _billService = billService;
        }

        public async Task AppointmentNotification(int appointmentID)
        {
            var appInfo = await _appointmentService.GetAppointmentByID(appointmentID);

            if (appInfo == null) return;

            var patientInfo = await _patientService.GetPatientById(appInfo.PatientID);

            if (patientInfo == null) return;

            using var message = new MailMessage
            {
                From = new MailAddress(senderAddress, senderName),
                Subject = "Appointment Schedule",
                Body = $@"Hello {patientInfo.FirstName},

We hope this message finds you well.

This email is to inform you that your appointment has been scheduled:

📅 Date/Time: {appInfo.StartTime}
⏱ Duration: {appInfo.DurationMinutes} minutes
👩‍⚕️ Doctor: {appInfo.DoctorName}
🧑‍🤝‍🧑 Patient: {appInfo.PatientName}

Looking forward to seeing you.

Regards,
CareLite Team"
            };

            message.To.Add(patientInfo.Email!);

            using var smtp = new SmtpClient("sendmail.curemd.com", 25)
            {
                UseDefaultCredentials = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            await smtp.SendMailAsync(message);
        }

        public async Task TransactionNotification(int paymentID)
        {
            var payment = await _billService.GetPaymentById(paymentID);
            if (payment == null) return;

            var bill = await _billService.GetBillByID(payment.BillID);
            if (bill == null) return;

            var patientInfo = await _patientService.GetPatientById(bill.PatientID);
            if (patientInfo == null) return;

            using var message = new MailMessage
            {
                From = new MailAddress(senderAddress, senderName),
                Subject = "Payment Confirmation",
                Body = $@"Hello {patientInfo.FirstName},

We have successfully received your payment.

💳 Payment ID: {payment.PaymentID}
💵 Amount Paid: {payment.Amount:C}
📄 Bill ID: {bill.BillID}
📅 Date: {DateTime.Now}

Your remaining pending amount is {bill.PendingAmount:C}.
Bill Status: {bill.Status}

Thank you for your payment.

Regards,
CareLite Team"
            };

            message.To.Add(patientInfo.Email!);

            using var smtp = new SmtpClient("sendmail.curemd.com", 25)
            {
                UseDefaultCredentials = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            await smtp.SendMailAsync(message);
        }
    }
}

