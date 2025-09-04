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

		public UserAuthRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public UserAuthDT GetByUsername(string username)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			using (SqlCommand cmd = new SqlCommand(SQLNamedQueries.SpGetUserByUsername, conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
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


