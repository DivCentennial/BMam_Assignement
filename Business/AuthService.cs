using System.Security.Cryptography;
using System.Text;
using MariApps.MS.Training.MSA.EmployeeMS.Business.Contracts;
using MariApps.MS.Training.MSA.EmployeeMS.DataCarrier;
using MariApps.MS.Training.MSA.EmployeeMS.Repository.Contracts.Repositories;

namespace MariApps.MS.Training.MSA.EmployeeMS.Business
{
	public class AuthService : IAuthService
	{
		private readonly IUserAuthRepository _userAuthRepository;

		public AuthService(IUserAuthRepository userAuthRepository)
		{
			_userAuthRepository = userAuthRepository;
		}

		public UserAuthEntity Login(string username, string password)
		{
			var user = _userAuthRepository.GetByUsername(username);
			if (user == null) return null;
			if (!VerifyPassword(password, user.PasswordHash)) return null;
			return new UserAuthEntity
			{
				UserId = user.UserId,
				UserName = user.Username,
				PasswordHash = user.PasswordHash,
				Role = user.Role,
				EmployeeId = user.EmployeeId
			};
		}

		private static bool VerifyPassword(string plain, string storedHash)
		{
			// For demo: support plain SHA256 hashes. Replace with bcrypt/argon2 in prod.
			using var sha = SHA256.Create();
			var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(plain));
			var hex = Convert.ToHexString(bytes);
			return string.Equals(hex, storedHash, StringComparison.OrdinalIgnoreCase)
				|| string.Equals(plain, storedHash, StringComparison.Ordinal); // fallback if DB stores plain for training
		}
	}
}


