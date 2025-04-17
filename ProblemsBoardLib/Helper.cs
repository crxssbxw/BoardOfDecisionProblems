using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ProblemsBoardLib
{
    /// <summary>
    /// Вспомогательный статический класс для упрощения работы с данными
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Метод для копирования значений свойств одного объекта в другой объект
        /// </summary>
        /// <param name="source">Объект-источник</param>
        /// <param name="destination">Объект-назначение</param>
        public static void CopyTo(object source, object destination)
        {
            Type sType = source.GetType();
            Type dType = destination.GetType();

            if (sType == null || dType == null)
            {
                throw new ArgumentNullException();
            }

            if (sType != dType)
            {
                throw new Exception("Различные типы объектов");
            }

            foreach (var prop in sType.GetProperties()) 
            {
                var dprop = dType.GetProperty(prop.Name);
                var value = prop.GetValue(source);
                dprop.SetValue(destination, value);
            }
        }
    }
}
