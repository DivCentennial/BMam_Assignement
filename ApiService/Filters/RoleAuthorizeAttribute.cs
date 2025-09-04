using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MariApps.MS.Training.MSA.EmployeeMS.ApiService.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public sealed class RoleAuthorizeAttribute : Attribute, IAsyncActionFilter
	{
		private readonly HashSet<string> _allowedRoles;

		public RoleAuthorizeAttribute(params string[] roles)
		{
			_allowedRoles = new HashSet<string>(roles.Select(r => r?.Trim() ?? string.Empty), StringComparer.OrdinalIgnoreCase);
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			// Minimal training approach: role is provided via X-Role header by the client after login
			var role = context.HttpContext.Request.Headers["X-Role"].FirstOrDefault();
			if (string.IsNullOrWhiteSpace(role) || (_allowedRoles.Count > 0 && !_allowedRoles.Contains(role)))
			{
				context.Result = new ForbidResult();
				return;
			}

			await next();
		}
	}
}


