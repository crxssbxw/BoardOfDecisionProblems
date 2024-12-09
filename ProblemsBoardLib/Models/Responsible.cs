using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    /// <summary>
    /// Модель Ответственных
    /// </summary>
    public class Responsible
    {
        /// <summary>
        /// Идентификатор ответственного
        /// </summary>
        public int ResponsibleId { get; set; }
        /// <summary>
        /// Идентификатор рабочего, назначаемого ответственным
        /// </summary>
        public int WorkerId { get; set; }
        /// <summary>
        /// Идентификатор отдела, в котором назначается ответственный
        /// </summary>
        public int DepartmentId { get; set; }
        /// <summary>
        /// Флаг, является ли ответственный текущим
        /// </summary>
        public bool IsCurrent { get; set; }
<<<<<<< HEAD

        /// <summary>
        /// Логин ответственного, null, если IsCurrent = false
        /// </summary>
        public string? Login { get; set; }
        /// <summary>
        /// Пароль ответственного, null, если IsCurrent = false
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Ссылка на отдел
        /// </summary>
=======
        public string Login { get; set; }
        public string Password { get; set; }
>>>>>>> 2c81db8 (Added models for Log, Report and Log Events, created base viewmodel for problems)
        public Department Department { get; set; }
        /// <summary>
        /// Ссылка на рабочего
        /// </summary>
        public Worker Worker { get; set; }
        /// <summary>
        /// Ссылка на проблему
        /// </summary>
        public ICollection<Problem> Problem { get; set; }

        [NotMapped]
        public string DepartmentView { get => $"[{Department.ViewerNumber}] - {Department.Name}"; }
        
        [NotMapped]
        public string WorkerView { get => $"{Worker.SecondName} {Worker.FirstName}"; }
        /// <summary>
        /// Представление объекта ответственного в виде строки
        /// </summary>
        /// <returns>Сторка вида "[Отдел] Фамилия Имя"</returns>
        public override string ToString()
        {
            return $"{Worker.Department} {Worker.SecondName} {Worker.FirstName}";
        }
    }
}
