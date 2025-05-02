using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public int? DaysLeft
        {
            get
            {
                if (DecisionTime >= Theme?.DaysToDecide) return 0;
                else return Theme?.DaysToDecide - DecisionTime;
            }
        }

        [NotMapped]
        public Visibility IsDecided
        {
            get
            {
                if (!string.IsNullOrEmpty(Description)) return Visibility.Collapsed;
                return Visibility.Visible;
            }
        }

        [NotMapped]
        public string? ThemeName { get => Theme?.Name; }

        [NotMapped]
        public string? ResponsibleName { get => $"{Responsible.Worker.FirstName} {Responsible.Worker.SecondName} {Responsible.Worker.Post}"; }

        [NotMapped]
        public string? DepartmentName { get => $"[{Department.ViewerNumber}] {Department.Name}"; }

        public Theme? Theme { get; set; }
        public Responsible? Responsible { get; set; }
        public Department? Department { get; set; }
    }
}
