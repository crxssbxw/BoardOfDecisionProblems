using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardOfDecisionProblems.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        
        public string? ViewerNumber { get; set; }
        public string? Name { get; set; }

        public override string ToString()
        {
            return $"[{ViewerNumber}] {Name}";
        }
        
        public ICollection<Worker> Workers { get; set; }
        public ICollection<Responsible> Responsibles { get; set; }
        public ICollection<Problem> Problems { get; set; }
    }
}
