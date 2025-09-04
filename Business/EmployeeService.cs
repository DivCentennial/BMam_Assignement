using MariApps.MS.Training.MSA.EmployeeMS.Business.Contracts;
using MariApps.MS.Training.MSA.EmployeeMS.DataCarrier;
using MariApps.MS.Training.MSA.EmployeeMS.DataModel;
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

        public EmployeePersonalEntity GetEmployeeById(int employeeId)
        {
            var model = _employeeRepository.GetEmployeeById(employeeId);
            if (model == null) return null;
            return new EmployeePersonalEntity
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
            };
        }

        public void AddEmployee(EmployeePersonalEntity personal, EmployeeProfessionalEntity professional)
        {
            var personalDt = new EmployeePersonalDT
            {
                EmployeeId = personal.EmployeeId,
                FullName = personal.FullName,
                Gender = personal.Gender,
                DOB = personal.DOB,
                Age = personal.Age,
                Address = personal.Address,
                ContactNo = personal.ContactNo,
                Email = personal.Email,
                ProfileImageUrl = personal.ProfileImageUrl
            };
            var professionalDt = new EmployeeProfessionalDT
            {
                EmployeeId = professional.EmployeeId,
                Designation = professional.Designation,
                Department = professional.Department,
                Qualification = professional.Qualification,
                Experience = professional.Experience,
                Skill = professional.Skill
            };
            _employeeRepository.AddEmployee(personalDt, professionalDt);
        }

        public void UpdateEmployee(EmployeePersonalEntity personal, EmployeeProfessionalEntity professional)
        {
            var personalDt = new EmployeePersonalDT
            {
                EmployeeId = personal.EmployeeId,
                FullName = personal.FullName,
                Gender = personal.Gender,
                DOB = personal.DOB,
                Age = personal.Age,
                Address = personal.Address,
                ContactNo = personal.ContactNo,
                Email = personal.Email,
                ProfileImageUrl = personal.ProfileImageUrl
            };
            var professionalDt = new EmployeeProfessionalDT
            {
                EmployeeId = professional.EmployeeId,
                Designation = professional.Designation,
                Department = professional.Department,
                Qualification = professional.Qualification,
                Experience = professional.Experience,
                Skill = professional.Skill
            };
            _employeeRepository.UpdateEmployee(personalDt, professionalDt);
        }

        public void DeleteEmployee(int employeeId)
        {
            _employeeRepository.DeleteEmployee(employeeId);
        }

        public void UpdateEmployeeImageUrl(int employeeId, string profileImageUrl)
        {
            _employeeRepository.UpdateEmployeeImageUrl(employeeId, profileImageUrl);
        }
    }
}
