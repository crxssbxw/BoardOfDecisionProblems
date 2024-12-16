using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProblemsBoardLib.Models
{
    /// <summary>
    /// Модель Проблем
    /// </summary>
    public class Problem
    {
        /// <summary>
        /// Идентификатор проблемы
        /// </summary>
        public int ProblemId { get; set; }
        /// <summary>
        /// Дата создания проблемы
        /// </summary>
        public DateTime DateOccurance { get; set; }
<<<<<<< HEAD:ProblemsBoardLib/Models/Problem.cs
        public string? Status { get; set; }
=======
        /// <summary>
        /// Статус проблемы
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Описание проблемы
        /// </summary>
>>>>>>> 7ec80ee (Added XML comments):BoardOfDecisionProblems/Models/Problem.cs
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Дата решения проблемы
        /// </summary>
        public DateTime? DateElimination { get; set; }
        /// <summary>
        /// Описание решения проблемы
        /// </summary>
        public string? Decision { get; set; }
        /// <summary>
        /// Идентификатор ответственного за решение проблемы
        /// </summary>
        public int? ResponsibleId { get; set; }
<<<<<<< HEAD:ProblemsBoardLib/Models/Problem.cs
        public int? WorkerId { get; set; }
=======
        /// <summary>
        /// Идентификатор Отдела, в котором возникла проблемы
        /// </summary>
>>>>>>> 7ec80ee (Added XML comments):BoardOfDecisionProblems/Models/Problem.cs
        public int? DepartmentId { get; set; }
        /// <summary>
        /// Идентификатор темы
        /// </summary>
        public int? ThemeId { get; set; }
<<<<<<< HEAD:ProblemsBoardLib/Models/Problem.cs

=======
        
        /// <summary>
        /// Время решения проблемы (в БД не записывается)
        /// </summary>
>>>>>>> 7ec80ee (Added XML comments):BoardOfDecisionProblems/Models/Problem.cs
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

        /// <summary>
        /// Сколько осталось времени на решение
        /// </summary>
        [NotMapped]
        public int? DaysLeft
        {
            get
            {
                if (DecisionTime >= Theme.DaysToDecide) return 0;
                else return Theme.DaysToDecide - DecisionTime;
            }
        }

        /// <summary>
        /// Отображаемый статус (в БД не записывается)
        /// </summary>
        [NotMapped]
        public int? DaysLeft
        {
            get
            {
                if (DecisionTime >= Theme?.DaysToDecide) return 0;
                else return Theme?.DaysToDecide - DecisionTime;
            }
        }
<<<<<<< HEAD:ProblemsBoardLib/Models/Problem.cs
=======
        
        /// <summary>
        /// Название темы
        /// </summary>
        [NotMapped]
        public string ThemeName { get => Theme.Name; }
>>>>>>> 7ec80ee (Added XML comments):BoardOfDecisionProblems/Models/Problem.cs

        /// <summary>
        /// Имя ответственного
        /// </summary>
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
        public Visibility IsNew
        {
            get
            {
                if (DateOccurance.Date == DateTime.Today) return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        [NotMapped]
        public string? ThemeName { get => Theme?.Name; }

        [NotMapped]
        public string? ResponsibleName { get => $"{Responsible.Worker.FirstName} {Responsible.Worker.SecondName} {Responsible.Worker.Post}"; }

        [NotMapped]
        public string? DepartmentName { get => $"[{Department.ViewerNumber}] {Department.Name}"; }

        /// <summary>
        /// Ссылка на тему
        /// </summary>
        public Theme? Theme { get; set; }
        /// <summary>
        /// Ссылка на ответственного
        /// </summary>
        public Responsible? Responsible { get; set; }
<<<<<<< HEAD:ProblemsBoardLib/Models/Problem.cs
        public Worker? Worker { get; set; }
=======
        /// <summary>
        /// Ссылка на отдел
        /// </summary>
>>>>>>> 7ec80ee (Added XML comments):BoardOfDecisionProblems/Models/Problem.cs
        public Department? Department { get; set; }
    }
}
