using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
<<<<<<< HEAD:ProblemsBoardLib/Models/Admin.cs
    public class Admin
    {
        public int AdminId { get; set; }
=======
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
>>>>>>> 7ec80ee (Added XML comments):BoardOfDecisionProblems/Models/User.cs
        public string Login { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// !!!ХРАНЕНИЕ ПАРОЛЯ В ОТКРЫТОМ ВИДЕ НАРУШАЕТ ТРЕБОВАНИЕ БЕЗОПАСНОСТИ!!!
        /// СОЗДАНО ИСКЛЮЧИТЕЛЬНО В ДЕМОНСТРАТИВНЫХ ЦЕЛЯХ
        /// </summary>
        public string Password { get; set; }
<<<<<<< HEAD:ProblemsBoardLib/Models/Admin.cs
=======
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
>>>>>>> 7ec80ee (Added XML comments):BoardOfDecisionProblems/Models/User.cs

        /// <summary>
        /// Ссылка на отдел
        /// </summary>
        public Department? Department { get; set; }
<<<<<<< HEAD:ProblemsBoardLib/Models/Admin.cs
        public int? DepartmentId { get; set; }
=======
        /// <summary>
        /// Ссылка на работника
        /// </summary>
        public Worker? Worker { get; set; }
>>>>>>> 7ec80ee (Added XML comments):BoardOfDecisionProblems/Models/User.cs
    }
}
