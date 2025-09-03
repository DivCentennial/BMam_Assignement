using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariApps.MS.Training.MSA.EmployeeMS.DataModel
{
    public class UserAuth
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }   // match DB column
        public string Role { get; set; } = "User";
        public int? EmployeeId { get; set; }       // nullable
    }


}
