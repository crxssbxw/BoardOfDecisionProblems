using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardOfDecisionProblems.Models
{
    /// <summary>
    /// Модель отдела
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

        /// <summary>
        /// Пользователи, связанные с этим отделом
        /// </summary>
        public ICollection<User> Users { get; set; }

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
    }
}
