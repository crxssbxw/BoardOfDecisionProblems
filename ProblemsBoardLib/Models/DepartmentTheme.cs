using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    public class DepartmentTheme
    {
        public int DepartmentId { get; set; }
        public int ThemeId { get; set; }
        public Department Department { get; set; }
        public Theme Theme { get; set; }
    }
}
