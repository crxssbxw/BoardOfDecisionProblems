using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardOfDecisionProblems.RoleModel
{
    /// <summary>
    /// Базовый класс для роли пользователя
    /// </summary>
    public class UserRole
    {
        private string _Login;
        public string Login { get => _Login; set => _Login = value; }

        private string _Password;
        public string Password { get => _Password; set => _Password = value; }
    }
}
