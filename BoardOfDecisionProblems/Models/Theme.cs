using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardOfDecisionProblems.Models
{
    public class Theme
    {
        public int ThemeId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<Problem> Problems { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
