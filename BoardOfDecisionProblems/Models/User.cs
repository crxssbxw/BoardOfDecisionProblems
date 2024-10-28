using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardOfDecisionProblems.Models
{
    /// <summary>
    /// Модель Пользователей
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// !!!ХРАНЕНИЕ ПАРОЛЯ В ОТКРЫТОМ ВИДЕ НАРУШАЕТ ТРЕБОВАНИЕ БЕЗОПАСНОСТИ!!!
        /// СОЗДАНО ИСКЛЮЧИТЕЛЬНО В ДЕМОНСТРАТИВНЫХ ЦЕЛЯХ
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Роль пользователя
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// Идентификатор работника (если привязан к пользователю)
        /// </summary>
        public int? WorkerId { get; set; }
        /// <summary>
        /// Идентификатор отдела (если привязан к пользователю)
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Ссылка на отдел
        /// </summary>
        public Department? Department { get; set; }
        /// <summary>
        /// Ссылка на работника
        /// </summary>
        public Worker? Worker { get; set; }
    }
}
