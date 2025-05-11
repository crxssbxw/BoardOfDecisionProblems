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
<<<<<<< HEAD
        /// <summary>
        /// Идентификатор Отдела, в котором возникла проблемы
        /// </summary>
=======
        public int? WorkerId { get; set; }
>>>>>>> 3952904 (Added more difference between roles, added roles enum, added new role: header worker; added email sender class)
        public int? DepartmentId { get; set; }
        /// <summary>
        /// Идентификатор темы
        /// </summary>
        public int? ThemeId { get; set; }
<<<<<<< HEAD
        
        /// <summary>
        /// Время решения проблемы (в БД не записывается)
        /// </summary>
=======

>>>>>>> ce1a19b (Added themes view in menu, adding themes to db, model changes)
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
<<<<<<< HEAD
        
        /// <summary>
        /// Название темы
        /// </summary>
=======

        [NotMapped]
        public Visibility IsDecided
        {
            get
            {
                if (!string.IsNullOrEmpty(Description)) return Visibility.Collapsed;
                return Visibility.Visible;
            }
        }

>>>>>>> ce1a19b (Added themes view in menu, adding themes to db, model changes)
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

        /// <summary>
        /// Имя ответственного
        /// </summary>
        [NotMapped]
        public string? ResponsibleName { get => $"{Responsible.Worker.FirstName} {Responsible.Worker.SecondName} {Responsible.Worker.Post}"; }

<<<<<<< HEAD
        /// <summary>
        /// Ссылка на тему
        /// </summary>
=======
        [NotMapped]
        public string? DepartmentName { get => $"[{Department.ViewerNumber}] {Department.Name}"; }

>>>>>>> 081a081 (Added Startup Window)
        public Theme? Theme { get; set; }
        /// <summary>
        /// Ссылка на ответственного
        /// </summary>
        public Responsible? Responsible { get; set; }
<<<<<<< HEAD
        /// <summary>
        /// Ссылка на отдел
        /// </summary>
=======
        public Worker? Worker { get; set; }
>>>>>>> 3952904 (Added more difference between roles, added roles enum, added new role: header worker; added email sender class)
        public Department? Department { get; set; }

        public override string ToString()
        {
            return $"[{ProblemId}] {Description} ({DateOccurance})";
        }
    }
}
