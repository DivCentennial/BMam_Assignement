
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

            builder.Services.RegisterServiceDependecies();

            builder.InitMSCoreService();

            var app = builder.Build();

            app.ConfigureMsRequestPineline();

            app.Run();
        }
    }
}