using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Cryptography;

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

		/// <summary>
		/// Шифрует строку алгоритмом SHA256
		/// </summary>
		/// <param name="str">Шифруемая строка</param>
		/// <returns>Зашифровання строка</returns>
		public static string EncryptString(string str)
        {
			SHA256 sha256 = SHA256.Create();
			byte[] bytesstr = Encoding.UTF8.GetBytes(str);
			byte[] hash = sha256.ComputeHash(bytesstr);
			string hashString = string.Empty;
			foreach (byte x in hash)
			{
				hashString += String.Format("{0:x2}", x);
			}
			return hashString;
		}
    }
}
