using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariApps.MS.Training.MSA.EmployeeMS.Repository
{
    internal static class SQLNamedQueries
    {
        public const string SpGetAllEmployees = "sp_GetAllEmployees";
        public const string SpGetEmployeeById = "sp_GetEmployeeById";
        public const string SpAddEmployeePersonal = "sp_AddEmployeePersonal";
        public const string SpAddEmployeeProfessional = "sp_AddEmployeeProfessional";
        public const string SpUpdateEmployeePersonal = "sp_UpdateEmployeePersonal";
        public const string SpUpdateEmployeeProfessional = "sp_UpdateEmployeeProfessional";
        public const string SpDeleteEmployee = "sp_DeleteEmployee";
        public const string SpUpdateEmployeeImageUrl = "sp_UpdateEmployeeImageUrl";

        public const string SpGetUserByUsername = "sp_GetUserByUsername";

        public static string WithSchema(this string procName, string schema)
        {
            if (string.IsNullOrWhiteSpace(schema)) schema = "dbo";
            return $"[{schema}].{procName}";
        }

    }
}
