using careliteBackend.DBHelper;
using careliteBackend.Repository;
using careliteBackend.Repository.PatientRepository;
using careliteBackend.Services;
using careliteBackend.Services.DoctorService;
using careliteBackend.Services.Interfaces;
using careliteBackend.Services.PatientService;

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

            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IJwtTokenService,JwtTokenService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService,DoctorService>();
            services.AddScoped<IAuth, AuthService>();
           return services;
        }
    }
}
