using MariApps.MS.Training.MSA.EmployeeMS.DataCarrier;

namespace MariApps.MS.Training.MSA.EmployeeMS.Business.Contracts
{
	public interface IAuthService
	{
		UserAuthEntity Login(string username, string password);
	}
}


