using MariApps.MS.Training.MSA.EmployeeMS.DataModel;
using System.Collections.Generic;

namespace MariApps.MS.Training.MSA.EmployeeMS.Repository.Contracts.Repositories
{
	public interface IUserAuthRepository
	{
		UserAuthDT GetByUsername(string username);
	}
}
