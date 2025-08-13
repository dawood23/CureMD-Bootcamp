using API_Demo.Repository;
using Web_API_Assignment.DBScripts;
using Web_API_Assignment.Repository;
using Web_API_Assignment.Services;

namespace Web_API_Assignment.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection Dependency_Injection(this IServiceCollection services)
        {

            services.AddScoped<DbHelper>();

            services.AddScoped<IVisitTypeRepository, VisitTypeRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IVisitRepository, VisitRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IActivityLogRepository, ActivityLogRepository>();

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IVisitTypeService, VisitTypeService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IVisitService, VisitService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();

            return services;
        }
    }
}
