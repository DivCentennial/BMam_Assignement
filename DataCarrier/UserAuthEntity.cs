using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariApps.MS.Training.MSA.EmployeeMS.DataCarrier
{
    public class UserAuthEntity
    {
        public int UserId { get; set; }

        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public int? EmployeeId { get; set; }

    }
}
