using MariApps.MS.Training.MSA.EmployeeMS.DataCarrier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MariApps.MS.Training.MSA.EmployeeMS.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public EmployeeController(ILogger<EmployeeController> logger)
        {
            logger.LogInformation("EmployeeMSController created");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }

        [HttpGet]
        public ActionResult<List<EmployeePersonalEntity>> GetAll()
        {
            return _employeeService.GetAllEmployees();
        }
    }
}
