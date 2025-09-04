using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariApps.MS.Training.MSA.EmployeeMS.DataModel
{
    public class EmployeeProfessionalDT
    {
        public int EmployeeId { get; set; }  
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Qualification { get; set; }
        public decimal? Experience { get; set; }
        public string Skill { get; set; }
    }
}
