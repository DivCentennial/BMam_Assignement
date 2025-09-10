using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariApps.MS.Training.MSA.EmployeeMS.DataModel
{
    public class EmployeePersonalDT
    {

        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public char Gender { get; set; } 
        public DateTime DOB { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Qualification { get; set; }
        public decimal? Experience { get; set; }
        public string Skill { get; set; }
        public string UploadDocURL { get; set; }

    }
}
