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
        private readonly string _schemaName;
        private readonly string _procSchema;

        public EmployeeRepository(string connectionString, string schemaName = "dbo", string procSchema = "dbo")
        {
            _connectionString = connectionString;
            _schemaName = string.IsNullOrWhiteSpace(schemaName) ? "dbo" : schemaName;
            _procSchema = string.IsNullOrWhiteSpace(procSchema) ? "dbo" : procSchema;
        }

        public List<EmployeePersonalDT> GetAllEmployees()
        {
            var employees = new List<EmployeePersonalDT>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = $@"
            SELECT 
                p.EmployeeId, 
                p.FullName, 
                p.Gender, 
                p.DOB, 
                p.Age, 
                p.Address, 
                p.ContactNo, 
                p.Email, 
                p.ProfileImageUrl,
                pr.Designation,
                pr.Department,
                pr.Qualification,
                pr.Experience,
                pr.Skill,
                pr.UploadDocURL
            FROM [{_schemaName}].[EmployeePersonal] p
            LEFT JOIN [{_schemaName}].[EmployeeProfessional] pr ON p.EmployeeId = pr.EmployeeId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
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
                                ProfileImageUrl = reader.IsDBNull(8) ? null : reader.GetString(8),
                                Designation = reader.IsDBNull(9) ? null : reader.GetString(9),
                                Department = reader.IsDBNull(10) ? null : reader.GetString(10),
                                Qualification = reader.IsDBNull(11) ? null : reader.GetString(11),
                                Experience = reader.IsDBNull(12) ? (int?)null : (int)reader.GetDecimal(12),
                                Skill = reader.IsDBNull(13) ? null : reader.GetString(13),
                                UploadDocURL = reader.IsDBNull(14) ? null : reader.GetString(14)  // Added this
                            });
                        }
                    }
                }
            }
            return employees;
        }



        public EmployeePersonalDT GetEmployeeById(int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Join with EmployeeProfessional table to get complete employee data
                string sql = $@"
                    SELECT 
                        p.EmployeeId, 
                        p.FullName, 
                        p.Gender, 
                        p.DOB, 
                        p.Age, 
                        p.Address, 
                        p.ContactNo, 
                        p.Email, 
                        p.ProfileImageUrl,
                        pr.Designation,
                        pr.Department,
                        pr.Qualification,
                        pr.Experience,
                        pr.Skill,
                        pr.UploadDocURL
                    FROM [{_schemaName}].[EmployeePersonal] p
                    LEFT JOIN [{_schemaName}].[EmployeeProfessional] pr ON p.EmployeeId = pr.EmployeeId
                    WHERE p.EmployeeId = @EmployeeId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
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
                                ProfileImageUrl = reader.IsDBNull(8) ? null : reader.GetString(8),
                                Designation = reader.IsDBNull(9) ? null : reader.GetString(9),
                                Department = reader.IsDBNull(10) ? null : reader.GetString(10),
                                Qualification = reader.IsDBNull(11) ? null : reader.GetString(11),
                                Experience = reader.IsDBNull(12) ? (decimal?)null : reader.GetDecimal(12),
                                Skill = reader.IsDBNull(13) ? null : reader.GetString(13),
                                UploadDocURL = reader.IsDBNull(14) ? null : reader.GetString(14)
                            };
                        }
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
                        // Get the next available EmployeeId if not provided or if it's 0
                        int employeeId = personal.EmployeeId;
                        if (employeeId <= 0)
                        {
                            string maxIdQuery = $"SELECT ISNULL(MAX(EmployeeId), 0) + 1 FROM [{_schemaName}].[EmployeePersonal]";
                            using (SqlCommand maxCmd = new SqlCommand(maxIdQuery, conn, tx))
                            {
                                employeeId = (int)maxCmd.ExecuteScalar();
                            }
                        }

                        // Update the EmployeeId in both objects
                        personal.EmployeeId = employeeId;
                        professional.EmployeeId = employeeId;

                        // Insert into EmployeePersonal table
                        string personalSql = $@"
                    INSERT INTO [{_schemaName}].[EmployeePersonal] 
                    (EmployeeId, FullName, Gender, DOB, Age, Address, ContactNo, Email, ProfileImageUrl)
                    VALUES
                    (@EmployeeId, @FullName, @Gender, @DOB, @Age, @Address, @ContactNo, @Email, @ProfileImageUrl)";

                        using (SqlCommand cmd = new SqlCommand(personalSql, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeId", personal.EmployeeId);
                            cmd.Parameters.AddWithValue("@FullName", personal.FullName);
                            cmd.Parameters.AddWithValue("@Gender", personal.Gender);
                            cmd.Parameters.AddWithValue("@DOB", personal.DOB);
                            cmd.Parameters.AddWithValue("@Age", personal.Age.HasValue ? (object)personal.Age.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(personal.Address) ? DBNull.Value : (object)personal.Address);
                            cmd.Parameters.AddWithValue("@ContactNo", personal.ContactNo);
                            cmd.Parameters.AddWithValue("@Email", personal.Email);
                            cmd.Parameters.AddWithValue("@ProfileImageUrl", string.IsNullOrEmpty(personal.ProfileImageUrl) ? DBNull.Value : (object)personal.ProfileImageUrl);

                            cmd.ExecuteNonQuery();
                        }

                        // Insert into EmployeeProfessional table
                        string professionalSql = $@"
                    INSERT INTO [{_schemaName}].[EmployeeProfessional] 
                    (EmployeeId, Designation, Department, Qualification, Experience, Skill, UploadDocURL)
                    VALUES
                    (@EmployeeId, @Designation, @Department, @Qualification, @Experience, @Skill, @UploadDocURL)";

                        using (SqlCommand cmd = new SqlCommand(professionalSql, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeId", professional.EmployeeId);
                            cmd.Parameters.AddWithValue("@Designation", professional.Designation);
                            cmd.Parameters.AddWithValue("@Department", professional.Department);
                            cmd.Parameters.AddWithValue("@Qualification", string.IsNullOrEmpty(professional.Qualification) ? DBNull.Value : (object)professional.Qualification);
                            cmd.Parameters.AddWithValue("@Experience", professional.Experience.HasValue ? (object)professional.Experience.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@Skill", string.IsNullOrEmpty(professional.Skill) ? DBNull.Value : (object)professional.Skill);
                            cmd.Parameters.AddWithValue("@UploadDocURL", string.IsNullOrEmpty(professional.UploadDocURL) ? DBNull.Value : (object)professional.UploadDocURL);

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
                        // Update EmployeePersonal table
                        string personalSql = $@"
                    UPDATE [{_schemaName}].[EmployeePersonal]
                    SET FullName = @FullName,
                        Gender = @Gender,
                        DOB = @DOB,
                        Age = @Age,
                        Address = @Address,
                        ContactNo = @ContactNo,
                        Email = @Email,
                        ProfileImageUrl = @ProfileImageUrl
                    WHERE EmployeeId = @EmployeeId";

                        using (SqlCommand cmd = new SqlCommand(personalSql, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeId", personal.EmployeeId);
                            cmd.Parameters.AddWithValue("@FullName", personal.FullName);
                            cmd.Parameters.AddWithValue("@Gender", personal.Gender);
                            cmd.Parameters.AddWithValue("@DOB", personal.DOB);
                            cmd.Parameters.AddWithValue("@Age", personal.Age.HasValue ? (object)personal.Age.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(personal.Address) ? DBNull.Value : (object)personal.Address);
                            cmd.Parameters.AddWithValue("@ContactNo", personal.ContactNo);
                            cmd.Parameters.AddWithValue("@Email", personal.Email);
                            cmd.Parameters.AddWithValue("@ProfileImageUrl", string.IsNullOrEmpty(personal.ProfileImageUrl) ? DBNull.Value : (object)personal.ProfileImageUrl);

                            cmd.ExecuteNonQuery();
                        }

                        // Update EmployeeProfessional table
                        string professionalSql = $@"
                    UPDATE [{_schemaName}].[EmployeeProfessional]
                    SET Designation = @Designation,
                        Department = @Department,
                        Qualification = @Qualification,
                        Experience = @Experience,
                        Skill = @Skill
                    WHERE EmployeeId = @EmployeeId";

                        using (SqlCommand cmd = new SqlCommand(professionalSql, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeId", professional.EmployeeId);
                            cmd.Parameters.AddWithValue("@Designation", professional.Designation);
                            cmd.Parameters.AddWithValue("@Department", professional.Department);
                            cmd.Parameters.AddWithValue("@Qualification", string.IsNullOrEmpty(professional.Qualification) ? DBNull.Value : (object)professional.Qualification);
                            cmd.Parameters.AddWithValue("@Experience", professional.Experience.HasValue ? (object)professional.Experience.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@Skill", string.IsNullOrEmpty(professional.Skill) ? DBNull.Value : (object)professional.Skill);

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
            {
                conn.Open();
                using (SqlTransaction tx = conn.BeginTransaction())
                {
                    try
                    {
                        // Delete EmployeeProfessional first due to FK
                        string professionalSql = $"DELETE FROM [{_schemaName}].[EmployeeProfessional] WHERE EmployeeId = @EmployeeId";
                        using (SqlCommand cmd = new SqlCommand(professionalSql, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete EmployeePersonal
                        string personalSql = $"DELETE FROM [{_schemaName}].[EmployeePersonal] WHERE EmployeeId = @EmployeeId";
                        using (SqlCommand cmd = new SqlCommand(personalSql, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
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

        public void UpdateEmployeeImageUrl(int employeeId, string profileImageUrl)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = $@"
                    UPDATE [{_schemaName}].[EmployeePersonal]
                    SET ProfileImageUrl = @ProfileImageUrl
                    WHERE EmployeeId = @EmployeeId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.Parameters.AddWithValue("@ProfileImageUrl", string.IsNullOrEmpty(profileImageUrl) ? DBNull.Value : (object)profileImageUrl);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }


}

