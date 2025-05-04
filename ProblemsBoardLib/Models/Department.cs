using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    /// <summary>
    /// Модель Отдела
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Уникальный номер Отдела
        /// </summary>
        public int DepartmentId { get; set; }
        
        /// <summary>
        /// Уникальный номер Отдела, видимый и редактируемый
        /// </summary>
        public string? ViewerNumber { get; set; }
        /// <summary>
        /// Название Отдела
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Приведение объекта в строковую форму
        /// </summary>
        /// <returns>Строка типа "[Видимый номер] Название"</returns>
        public override string ToString()
        {
            return $"[{ViewerNumber}] {Name}";
        }
<<<<<<< HEAD

        /// <summary>
        /// Рабочие, связанные с этим отделом
        /// </summary>
=======
        
<<<<<<< HEAD
>>>>>>> 0e3fd82 (Edited database model and removed Users functions)
=======
        public Admin? Admin { get; set; }
        public int? AdminId { get; set; }

<<<<<<< HEAD
>>>>>>> 210d09c (Admin Authorization)
        public ICollection<Worker> Workers { get; set; }

        /// <summary>
        /// Ответственные, связанные этим отделом
        /// </summary>
        public ICollection<Responsible> Responsibles { get; set; }

        /// <summary>
        /// Проблемы, связанные с этим отделом
        /// </summary>
        public ICollection<Problem> Problems { get; set; }
        public ICollection<Theme> Themes { get; set; }
=======
        public ICollection<Worker> Workers { get; set; } = [];
        public ICollection<Responsible> Responsibles { get; set; } = [];
        public ICollection<Problem> Problems { get; set; } = [];
        public ICollection<Theme> Themes { get; set; } = [];
>>>>>>> 1be2c83 (Added reporting window, more functional statistic and reporting for it)
    }
}
