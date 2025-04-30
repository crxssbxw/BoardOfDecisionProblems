using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    /// <summary>
    /// Модель Темы
    /// </summary>
    public class Theme
    {
<<<<<<< HEAD
        /// <summary>
        /// Идентификатор темы
        /// </summary>
        public int ThemeId { get; set; }
        /// <summary>
        /// Название темы
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание темы
        /// </summary>
        public string? Description { get; set; }
        public int DaysToDecide { get; set; }
        public Department? Department { get; set; }
        public int? DepartmentId { get; set; } 

        /// <summary>
        /// Количество дней на решение проблемы этой темы
        /// </summary>
        public int DaysToDecide { get; set; }

        /// <summary>
        /// Проблемы, связанные с этой темой
        /// </summary>
        public ICollection<Problem> Problems { get; set; }

<<<<<<< HEAD
        /// <summary>
        /// Представление объекта в виде строки
        /// </summary>
        /// <returns>Строка вида "Название"</returns>
=======
=======
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
>>>>>>> c7d0ed8 (Board Properties with responsible set and themes view)
        public string Error => throw new NotImplementedException();

>>>>>>> ce1a19b (Added themes view in menu, adding themes to db, model changes)
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
