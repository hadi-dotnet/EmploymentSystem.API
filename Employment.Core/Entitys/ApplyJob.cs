using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class ApplyJob
    {
        public int ID { get; set; }
        public string? EmployeeID { get; set; }
        public int? JobID { get; set; }
        public Employees? Employee { get; set; }
        public Jobs? Jobs { get; set; }

    }
}
