using MariApps.MS.Training.MSA.EmployeeMS.Business.Contracts;
using MariApps.MS.Training.MSA.EmployeeMS.DataCarrier;
using Microsoft.AspNetCore.Mvc;

namespace MariApps.MS.Training.MSA.EmployeeMS.ApiService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		public class LoginRequest
		{
			public string Username { get; set; }
			public string Password { get; set; }
		}

		[HttpPost("login")]
		public ActionResult<UserAuthEntity> Login([FromBody] LoginRequest request)
		{
			if (string.IsNullOrWhiteSpace(request?.Username) || string.IsNullOrWhiteSpace(request?.Password))
				return BadRequest("Username and password are required.");

			var user = _authService.Login(request.Username, request.Password);
			if (user == null) return Unauthorized("Invalid credentials");
			return Ok(user);
		}
	}
}


