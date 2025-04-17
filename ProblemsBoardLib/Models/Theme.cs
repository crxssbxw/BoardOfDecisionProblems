using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    public class Theme
    {
        public int ThemeId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int DaysToDecide { get; set; }
        public Department? Department { get; set; }
        public int? DepartmentId { get; set; } 

        public ICollection<Problem> Problems { get; set; }

        public string Error => throw new NotImplementedException();

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
