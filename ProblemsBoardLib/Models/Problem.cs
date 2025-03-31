using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
<<<<<<< HEAD
        /// <summary>
        /// Статус проблемы
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Описание проблемы
        /// </summary>
=======
        public string? Status { get; set; }
>>>>>>> 2c81db8 (Added models for Log, Report and Log Events, created base viewmodel for problems)
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
        /// <summary>
        /// Идентификатор Отдела, в котором возникла проблемы
        /// </summary>
        public int? DepartmentId { get; set; }
        /// <summary>
        /// Идентификатор темы
        /// </summary>
        public int? ThemeId { get; set; }
        
        /// <summary>
        /// Время решения проблемы (в БД не записывается)
        /// </summary>
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
        
        /// <summary>
        /// Название темы
        /// </summary>
        [NotMapped]
        public string ThemeName { get => Theme.Name; }

        /// <summary>
        /// Имя ответственного
        /// </summary>
        [NotMapped]
        public string ResponsibleName { get => $"{Responsible.Worker.FirstName} {Responsible.Worker.SecondName} {Responsible.Worker.Post}"; }

<<<<<<< HEAD
        /// <summary>
        /// Ссылка на тему
        /// </summary>
=======
        [NotMapped]
        public string DepartmentName { get => $"[{Department.ViewerNumber}] {Department.Name}"; }

>>>>>>> 081a081 (Added Startup Window)
        public Theme? Theme { get; set; }
        /// <summary>
        /// Ссылка на ответственного
        /// </summary>
        public Responsible? Responsible { get; set; }
        /// <summary>
        /// Ссылка на отдел
        /// </summary>
        public Department? Department { get; set; }

        public override string ToString()
        {
            return $"[{ProblemId}] {Description} ({DateOccurance})";
        }
    }
}
