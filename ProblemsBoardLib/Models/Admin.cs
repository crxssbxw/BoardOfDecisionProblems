using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public Department? Department { get; set; }
        public int? DepartmentId { get; set; }
    }
}
