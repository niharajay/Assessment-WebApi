using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeDetailsAPI.Models
{
    public class Employee
    {
        public string EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime JoinedDate { get; set; }
        public double Salary { get; set; }
        public string Branch { get; set; }
        public string Country { get; set; }

        public double PATETax { get; set; }

        public double NetPayAmount { get; set; }
    }
}