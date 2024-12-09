using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    public class Problem
    {
        public int ProblemId { get; set; }
        public DateTime DateOccurance { get; set; }
        public string? Status { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? DateElimination { get; set; }
        public string? Decision { get; set; }
        public int? ResponsibleId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ThemeId { get; set; }
        
        [NotMapped]
        public int? DecisionTime
        {
            get
            {
                if (DateElimination == null)
                {
                    return DateTime.Now.Subtract(DateOccurance).Days;
                }
                else return ((DateTime)DateElimination).Subtract(DateOccurance).Days;
            }
        }

        [NotMapped]
        public byte ViewStatus
        {
            get
            {
                if (Status == "Решено" || Status == "Решено оп." && DecisionTime < 20) return 1;
                if (Status == "Решено" || Status == "Решено оп." && DecisionTime >= 20) return 2;
                if (Status == "Решается" || Status == "Решается оп." && DecisionTime < 20) return 3;
                if (Status == "Решается" || Status == "Решается оп." && DecisionTime >= 20) return 4;
                else return 0;
            }
        }
        
        [NotMapped]
        public string ThemeName { get => Theme.Name; }

        [NotMapped]
        public string ResponsibleName { get => $"{Responsible.Worker.FirstName} {Responsible.Worker.SecondName} {Responsible.Worker.Post}"; }

        public Theme? Theme { get; set; }
        public Responsible? Responsible { get; set; }
        public Department? Department { get; set; }
    }
}
