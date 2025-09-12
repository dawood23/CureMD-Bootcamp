using careliteBackend.DBHelper;
using careliteBackend.DTOs;
using careliteBackend.FluentValidation;
using careliteBackend.Repository;
using careliteBackend.Repository.BillRepository;
using careliteBackend.Repository.PatientRepository;
using careliteBackend.Repository.VisitNoteRepository;
using careliteBackend.Repository.VisitRepository;
using careliteBackend.Services;
using careliteBackend.Services.AppointmentService;
using careliteBackend.Services.BillService;
using careliteBackend.Services.DoctorService;
using careliteBackend.Services.Interfaces;
using careliteBackend.Services.PatientService;
using careliteBackend.Services.VisitService;
using FluentValidation;

namespace careliteBackend.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection Dependency_Injection(this IServiceCollection services)
        {
            services.AddScoped<DataBaseConnection>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPatientRepository,PatientRepository>();
            services.AddScoped<IDoctorRepository,DoctorRepository>();
            services.AddScoped<IAppointmentRepository,AppointmentRepository>();
            services.AddScoped<IVisitRepository,VisitRepository>();
            services.AddScoped<IBillRepository,BillRepository>();

            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IJwtTokenService,JwtTokenService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService,DoctorService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IVisitService, VisitService>();
            services.AddScoped<IBillService,Billservice>();

            services.AddScoped<IValidator<AppointmentRequest>, AppointmentRequestValidation>();

           return services;
        }
    }
}
