using MariApps.MS.Training.MSA.EmployeeMS.DataCarrier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariApps.MS.Training.MSA.EmployeeMS.Business.Contracts
{
    public interface IEmployeeService
    {
        List<EmployeePersonalEntity> GetAllEmployees();
        EmployeePersonalEntity GetEmployeeById(int employeeId);
        void AddEmployee(EmployeePersonalEntity employeePersonal, EmployeeProfessionalEntity employeeProfessional);
        void UpdateEmployee(EmployeePersonalEntity employeePersonal, EmployeeProfessionalEntity employeeProfessional);
        void DeleteEmployee(int employeeId);

        // Add other business logic methods as needed


    }
}
