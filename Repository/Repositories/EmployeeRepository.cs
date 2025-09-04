using MariApps.MS.Training.MSA.EmployeeMS.DataModel;
using MariApps.MS.Training.MSA.EmployeeMS.Repository.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariApps.MS.Training.MSA.EmployeeMS.Repository.Repositories
{
       public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<EmployeePersonal> GetAllEmployees()
        {
            var employees = new List<EmployeePersonal>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllEmployees", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new EmployeePersonal
                        {
                            EmployeeId = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Gender = reader.GetString(2)[0],
                            DOB = reader.GetDateTime(3),
                            Age = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                            Address = reader.IsDBNull(5) ? null : reader.GetString(5),
                            ContactNo = reader.GetString(6),
                            Email = reader.GetString(7),
                            ProfileImageUrl = reader.IsDBNull(8) ? null : reader.GetString(8)
                        });
                    }
                }
            }
            return employees;
        }

        // Implement other methods similarly...

        public EmployeePersonal GetEmployeeById(int employeeId) { /* ... */ }
        public void AddEmployee(EmployeePersonal personal, EmployeeProfessional professional) { /* ... */ }
        public void UpdateEmployee(EmployeePersonal personal, EmployeeProfessional professional) { /* ... */ }
        public void DeleteEmployee(int employeeId) { /* ... */ }
    }


}

