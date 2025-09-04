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
        List<EmployeePersonal> GetAllEmployees();
        EmployeePersonal GetEmployeeById(int employeeId);
        void AddEmployee(EmployeePersonal employeePersonal, EmployeeProfessional employeeProfessional);
        void UpdateEmployee(EmployeePersonal employeePersonal, EmployeeProfessional employeeProfessional);
        void DeleteEmployee(int employeeId);

    }
}
