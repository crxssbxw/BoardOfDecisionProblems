using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    /// <summary>
    /// Модель Темы
    /// </summary>
    public class Theme
    {
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

        /// <summary>
        /// Количество дней на решение проблемы этой темы
        /// </summary>
        public int DaysToDecide { get; set; }

        /// <summary>
        /// Проблемы, связанные с этой темой
        /// </summary>
        public ICollection<Problem> Problems { get; set; }

        /// <summary>
        /// Представление объекта в виде строки
        /// </summary>
        /// <returns>Строка вида "Название"</returns>
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
