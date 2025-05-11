using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    public class Theme
    {
        [JsonIgnore]
        public int ThemeId { get; set; }

        [JsonPropertyName("ThemeName")]
        public string Name { get; set; }

        [JsonPropertyName("ThemeDescription")]
        public string? Description { get; set; }

        [JsonPropertyName("ThemeDays")]
        public int DaysToDecide { get; set; }

        [JsonIgnore]
        public Department? Department { get; set; }

        [JsonIgnore]
        public int? DepartmentId { get; set; }

        [JsonIgnore]
        public ICollection<Problem> Problems { get; set; }

        [JsonIgnore]
        public string Error => throw new NotImplementedException();

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
