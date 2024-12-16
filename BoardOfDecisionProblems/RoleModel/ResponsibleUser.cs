using BoardOfDecisionProblems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardOfDecisionProblems.RoleModel
{
    public class ResponsibleUser : UserRole
    {
        private Responsible _responsible;
        public Responsible Responsible { get => _responsible; set => _responsible = value; }
    }
}
