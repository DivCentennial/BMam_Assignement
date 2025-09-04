using MariApps.MS.Training.MSA.EmployeeMS.Business.Contracts;
using MariApps.MS.Training.MSA.EmployeeMS.DataCarrier;
using MariApps.MS.Training.MSA.EmployeeMS.Repository.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariApps.MS.Training.MSA.EmployeeMS.Business
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public List<EmployeePersonalEntity> GetAllEmployees()
        {
            var employeeModels = _employeeRepository.GetAllEmployees();
            var employeeEntities = new List<EmployeePersonalEntity>();
            foreach (var model in employeeModels)
            {
                employeeEntities.Add(new EmployeePersonalEntity
                {
                    EmployeeId = model.EmployeeId,
                    FullName = model.FullName,
                    Gender = model.Gender,
                    DOB = model.DOB,
                    Age = model.Age,
                    Address = model.Address,
                    ContactNo = model.ContactNo,
                    Email = model.Email,
                    ProfileImageUrl = model.ProfileImageUrl
                });
            }
            return employeeEntities;
        }

        // Implement other methods similarly...
        public EmployeePersonalEntity GetEmployeeById(int employeeId) { /* ... */ }
        public void AddEmployee(EmployeePersonalEntity personal, EmployeeProfessionalEntity professional) { /* ... */ }
        public void UpdateEmployee(EmployeePersonalEntity personal, EmployeeProfessionalEntity professional) { /* ... */ }
        public void DeleteEmployee(int employeeId) { /* ... */ }
    }
}
