using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardOfDecisionProblems.Models
{
    /// <summary>
    /// Модель работника
    /// </summary>
    public class Worker
    {
        /// <summary>
        /// Идентификатор Работника
        /// </summary>
        public int WorkerId { get; set; }
        /// <summary>
        /// Имя Работника
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия Работника
        /// </summary>
        public string SecondName { get; set; }
        /// <summary>
        /// Отчество Работника
        /// </summary>
        public string? MiddleName { get; set; }
        /// <summary>
        /// Должность работника
        /// </summary>
        public string Post { get; set; }
        /// <summary>
        /// Идентификатор отдела работника
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Ссылка на отдел
        /// </summary>
        public Department Department { get; set; }
        [NotMapped]
        public string DepartmentNumber
        {
            get => Department.ViewerNumber;
        }

        /// <summary>
        /// Пользователи, связанные с работником
        /// </summary>
        public ICollection<User>? Users { get; set; }
        /// <summary>
        /// Ответственные, связанные с работником
        /// </summary>
        public ICollection<Responsible>? Responsibles { get; set; }

        /// <summary>
        /// Представление объекта в виде строки
        /// </summary>
        /// <returns>Строка вида "Фамилия Имя"</returns>
        public override string ToString()
        {
            return $"{SecondName} {FirstName}";
        }
    }
}
