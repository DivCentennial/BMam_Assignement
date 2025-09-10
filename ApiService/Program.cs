using MariApps.MS.Training.MSA.EmployeeMS.ApiService.Extensions;
using MariApps.Framework.MS.Core.Extensions;
using MariApps.Framework.MS.Core.Middlewares;

namespace MariApps.MS.Training.MSA.EmployeeMS.ApiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add CORS services
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.RegisterServiceDependecies();

            builder.InitMSCoreService();

            var app = builder.Build();

            // Use CORS middleware
            app.UseCors("AllowAngularApp");

            app.ConfigureMsRequestPineline();

            app.Run();
        }
    }
}