using MariApps.MS.Training.MSA.EmployeeMS.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariApps.MS.Training.MSA.EmployeeMS.Repository.Contracts.Repositories
{
    public interface IEmployeeRepository
    {
        List<EmployeePersonalDT> GetAllEmployees();
        EmployeePersonalDT GetEmployeeById(int employeeId);
        void AddEmployee(EmployeePersonalDT employeePersonal, EmployeeProfessionalDT employeeProfessional);
        void UpdateEmployee(EmployeePersonalDT employeePersonal, EmployeeProfessionalDT employeeProfessional);
        void DeleteEmployee(int employeeId);
        void UpdateEmployeeImageUrl(int employeeId, string profileImageUrl);

    }
}
