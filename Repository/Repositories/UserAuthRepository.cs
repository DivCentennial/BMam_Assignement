using MariApps.MS.Training.MSA.EmployeeMS.DataModel;
using MariApps.MS.Training.MSA.EmployeeMS.Repository.Contracts.Repositories;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MariApps.MS.Training.MSA.EmployeeMS.Repository.Repositories
{
	public class UserAuthRepository : IUserAuthRepository
	{
		private readonly string _connectionString;
        private readonly string _schemaName;

		public UserAuthRepository(string connectionString, string schemaName = "dbo")
		{
			_connectionString = connectionString;
            _schemaName = string.IsNullOrWhiteSpace(schemaName) ? "dbo" : schemaName;
		}

		public UserAuthDT GetByUsername(string username)
		{
			try
			{
				return ExecuteGetByUsername(username, _schemaName);
			}
			catch (SqlException ex) when (ex.Number == 208 /* Invalid object name */ && !string.Equals(_schemaName, "dbo", StringComparison.OrdinalIgnoreCase))
			{
				// Fallback to dbo if configured schema is incorrect
				return ExecuteGetByUsername(username, "dbo");
			}
		}

		private UserAuthDT ExecuteGetByUsername(string username, string schema)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			using (SqlCommand cmd = new SqlCommand($"SELECT UserId, Username, PasswordHash, Role, EmployeeId FROM [{schema}].UserAuth WHERE Username = @Username", conn))
			{
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@Username", username);
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						return new UserAuthDT
						{
							UserId = reader.GetInt32(0),
							Username = reader.GetString(1),
							PasswordHash = reader.GetString(2),
							Role = reader.GetString(3),
							EmployeeId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4)
						};
					}
				}
			}
			return null;
		}
	}
}


