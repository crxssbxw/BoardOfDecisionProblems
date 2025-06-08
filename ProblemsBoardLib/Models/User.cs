using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        [NotMapped]
        public Roles EnumRole
        {
            get
            {
                switch (Role)
                {
                    case "Админ":
                        return Roles.RAdmin;
                    case "Ответственный по решению проблем":
                        return Roles.RResponsible;
                    case "Главный по сбору проблем":
                        return Roles.RHeaderWorker;
                    default:
                        return Roles.None;
                }
            }
        }
    }
}
