using MariApps.MS.Training.MSA.EmployeeMS.DataModel;
using MariApps.MS.Training.MSA.EmployeeMS.Repository;
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

        public List<EmployeePersonalDT> GetAllEmployees()
        {
            var employees = new List<EmployeePersonalDT>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLNamedQueries.SpGetAllEmployees, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new EmployeePersonalDT
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
        public EmployeePersonalDT GetEmployeeById(int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLNamedQueries.SpGetEmployeeById, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new EmployeePersonalDT
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
                        };
                    }
                }
            }
            return null;
        }

        public void AddEmployee(EmployeePersonalDT personal, EmployeeProfessionalDT professional)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlTransaction tx = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(SQLNamedQueries.SpAddEmployeePersonal, conn, tx))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EmployeeId", personal.EmployeeId);
                            cmd.Parameters.AddWithValue("@FullName", personal.FullName);
                            cmd.Parameters.AddWithValue("@Gender", personal.Gender);
                            cmd.Parameters.AddWithValue("@DOB", personal.DOB);
                            if (personal.Age.HasValue)
                                cmd.Parameters.AddWithValue("@Age", personal.Age.Value);
                            else
                                cmd.Parameters.AddWithValue("@Age", DBNull.Value);
                            if (string.IsNullOrEmpty(personal.Address))
                                cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("@Address", personal.Address);
                            cmd.Parameters.AddWithValue("@ContactNo", personal.ContactNo);
                            cmd.Parameters.AddWithValue("@Email", personal.Email);
                            if (string.IsNullOrEmpty(personal.ProfileImageUrl))
                                cmd.Parameters.AddWithValue("@ProfileImageUrl", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("@ProfileImageUrl", personal.ProfileImageUrl);
                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = new SqlCommand(SQLNamedQueries.SpAddEmployeeProfessional, conn, tx))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EmployeeId", professional.EmployeeId);
                            cmd.Parameters.AddWithValue("@Designation", professional.Designation);
                            cmd.Parameters.AddWithValue("@Department", professional.Department);
                            if (string.IsNullOrEmpty(professional.Qualification))
                                cmd.Parameters.AddWithValue("@Qualification", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("@Qualification", professional.Qualification);
                            if (professional.Experience.HasValue)
                                cmd.Parameters.AddWithValue("@Experience", professional.Experience.Value);
                            else
                                cmd.Parameters.AddWithValue("@Experience", DBNull.Value);
                            if (string.IsNullOrEmpty(professional.Skill))
                                cmd.Parameters.AddWithValue("@Skill", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("@Skill", professional.Skill);
                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateEmployee(EmployeePersonalDT personal, EmployeeProfessionalDT professional)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlTransaction tx = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(SQLNamedQueries.SpUpdateEmployeePersonal, conn, tx))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EmployeeId", personal.EmployeeId);
                            cmd.Parameters.AddWithValue("@FullName", personal.FullName);
                            cmd.Parameters.AddWithValue("@Gender", personal.Gender);
                            cmd.Parameters.AddWithValue("@DOB", personal.DOB);
                            if (personal.Age.HasValue)
                                cmd.Parameters.AddWithValue("@Age", personal.Age.Value);
                            else
                                cmd.Parameters.AddWithValue("@Age", DBNull.Value);
                            if (string.IsNullOrEmpty(personal.Address))
                                cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("@Address", personal.Address);
                            cmd.Parameters.AddWithValue("@ContactNo", personal.ContactNo);
                            cmd.Parameters.AddWithValue("@Email", personal.Email);
                            if (string.IsNullOrEmpty(personal.ProfileImageUrl))
                                cmd.Parameters.AddWithValue("@ProfileImageUrl", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("@ProfileImageUrl", personal.ProfileImageUrl);
                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = new SqlCommand(SQLNamedQueries.SpUpdateEmployeeProfessional, conn, tx))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EmployeeId", professional.EmployeeId);
                            cmd.Parameters.AddWithValue("@Designation", professional.Designation);
                            cmd.Parameters.AddWithValue("@Department", professional.Department);
                            if (string.IsNullOrEmpty(professional.Qualification))
                                cmd.Parameters.AddWithValue("@Qualification", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("@Qualification", professional.Qualification);
                            if (professional.Experience.HasValue)
                                cmd.Parameters.AddWithValue("@Experience", professional.Experience.Value);
                            else
                                cmd.Parameters.AddWithValue("@Experience", DBNull.Value);
                            if (string.IsNullOrEmpty(professional.Skill))
                                cmd.Parameters.AddWithValue("@Skill", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("@Skill", professional.Skill);
                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public void DeleteEmployee(int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(SQLNamedQueries.SpDeleteEmployee, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateEmployeeImageUrl(int employeeId, string profileImageUrl)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(SQLNamedQueries.SpUpdateEmployeeImageUrl, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                if (string.IsNullOrEmpty(profileImageUrl))
                    cmd.Parameters.AddWithValue("@ProfileImageUrl", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ProfileImageUrl", profileImageUrl);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }


}

