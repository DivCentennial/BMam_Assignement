using MariApps.Framework.Core.Abstractions.Contracts;
using MariApps.MS.Training.MSA.EmployeeMS.Business;
using MariApps.MS.Training.MSA.EmployeeMS.Business.Contracts;
using MariApps.MS.Training.MSA.EmployeeMS.Repository.Contracts.DbContext;
using MariApps.MS.Training.MSA.EmployeeMS.Repository.Contracts.Repositories;
using MariApps.MS.Training.MSA.EmployeeMS.Repository.DbContext;
using MariApps.MS.Training.MSA.EmployeeMS.Repository.Repositories;

namespace MariApps.MS.Training.MSA.EmployeeMS.ApiService.Extensions
{
    public static class DependencyRegistration
    {
        public static void RegisterServiceDependecies(this IServiceCollection services)
        {
            services.AddScoped<IAdoDbContext, AdoDbContext>(provider =>
            {
                IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
                ILogger<AdoDbContext> logger = provider.GetRequiredService<ILogger<AdoDbContext>>();
                IAuditLogsOrchestrator auditlog = provider.GetRequiredService<IAuditLogsOrchestrator>();
                IRequestCorrelationIdAccessor correlationIdAccessor = provider.GetRequiredService<IRequestCorrelationIdAccessor>();

                string? palConnectionString = configuration.GetConnectionString("PALConnectionString");

                if (string.IsNullOrEmpty(palConnectionString))
                    throw new ArgumentNullException("Connection string not found");

                return new AdoDbContext(palConnectionString, logger, auditlog, correlationIdAccessor);
            });

            services.AddScoped<ISampleRepository, SampleRepository>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>(provider =>
            {
                IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
                string? palConnectionString = configuration.GetConnectionString("PALConnectionString");
                string schema = configuration.GetValue<string>("DbSchema") ?? "dbo";
                string procSchema = configuration.GetValue<string>("ProcSchema") ?? schema;
                if (string.IsNullOrEmpty(palConnectionString))
                    throw new ArgumentNullException("Connection string not found");
                return new EmployeeRepository(palConnectionString, schema, procSchema);
            });

            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddScoped<IUserAuthRepository, UserAuthRepository>(provider =>
            {
                IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
                string? palConnectionString = configuration.GetConnectionString("PALConnectionString");
                string schema = configuration.GetValue<string>("DbSchema") ?? "dbo";
                if (string.IsNullOrEmpty(palConnectionString))
                    throw new ArgumentNullException("Connection string not found");
                return new UserAuthRepository(palConnectionString, schema);
            });
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
