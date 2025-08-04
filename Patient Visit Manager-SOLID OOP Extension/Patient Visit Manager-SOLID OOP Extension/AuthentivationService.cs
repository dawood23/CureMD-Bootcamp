using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public class AuthenticationService
    {
        private readonly ILogger _logger;
        private readonly INotificationService _notificationService;

        private const string AdminEmail = "admin@gmail.com";
        private const string AdminPassword = "12345";
        private const string ReceptionistEmail = "reception@gmail.com";
        private const string ReceptionistPassword = "1234";

        public AuthenticationService(ILogger logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        public bool AuthenticateAdmin(string email, string password)
        {
            bool isValid = email == AdminEmail && password == AdminPassword;
            _logger.Log(isValid ? "Admin Login" : "Admin Login Attempt", Roles.Admin, isValid);

            if (!isValid)
            {
                _notificationService.ShowError("Invalid Admin Credentials");
            }

            return isValid;
        }

        public bool AuthenticateReceptionist(string email, string password)
        {
            bool isValid = email == ReceptionistEmail && password == ReceptionistPassword;
            _logger.Log(isValid ? "Reception Login" : "Reception Login Attempt", Roles.Receptionist, isValid);

            if (!isValid)
            {
                _notificationService.ShowError("Invalid Receptionist Credentials");
            }

            return isValid;
        }
    }
}
