using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardOfDecisionProblems.Models
{
    public class Responsible
    {
        public int ResponsibleId { get; set; }
        public int WorkerId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsCurrent { get; set; }

        public Department Department { get; set; }
        public Worker Worker { get; set; }
        public ICollection<Problem> Problem { get; set; }

        [NotMapped]
        public string DepartmentView { get => $"[{Department.ViewerNumber}] - {Department.Name}"; }
        
        [NotMapped]
        public string WorkerView { get => $"{Worker.SecondName} {Worker.FirstName}"; }

        public override string ToString()
        {
            return $"{Worker.Department} {Worker.SecondName} {Worker.FirstName}";
        }
    }
}
