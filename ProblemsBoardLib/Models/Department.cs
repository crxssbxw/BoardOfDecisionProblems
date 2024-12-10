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
        
        public Admin? Admin { get; set; }
        public int? AdminId { get; set; }

<<<<<<< HEAD:ProblemsBoardLib/Models/Department.cs
        public ICollection<Worker> Workers { get; set; } = [];
        public ICollection<Responsible> Responsibles { get; set; } = [];
        public ICollection<Problem> Problems { get; set; } = [];
        public ICollection<Theme> Themes { get; set; } = [];
=======
        /// <summary>
        /// Рабочие, связанные с этим отделом
        /// </summary>
        public ICollection<Worker> Workers { get; set; }

        /// <summary>
        /// Ответственные, связанные этим отделом
        /// </summary>
        public ICollection<Responsible> Responsibles { get; set; }

        /// <summary>
        /// Проблемы, связанные с этим отделом
        /// </summary>
        public ICollection<Problem> Problems { get; set; }
>>>>>>> 7ec80ee (Added XML comments):BoardOfDecisionProblems/Models/Department.cs
    }
}
