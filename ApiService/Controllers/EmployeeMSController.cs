using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MariApps.MS.Training.MSA.EmployeeMS.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeMSController : ControllerBase
    {
        public EmployeeMSController(ILogger<EmployeeMSController> logger)
        {
            logger.LogInformation("EmployeeMSController created");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }
    }
}
