using careliteBackend.DBHelper;
using careliteBackend.Repository;
using careliteBackend.Repository.PatientRepository;
using careliteBackend.Repository.VisitNoteRepository;
using careliteBackend.Repository.VisitRepository;
using careliteBackend.Services;
using careliteBackend.Services.AppointmentService;
using careliteBackend.Services.DoctorService;
using careliteBackend.Services.Interfaces;
using careliteBackend.Services.PatientService;
using careliteBackend.Services.VisitService;

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

            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IJwtTokenService,JwtTokenService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService,DoctorService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IVisitService, VisitService>();

           return services;
        }
    }
}
