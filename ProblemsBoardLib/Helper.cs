using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Cryptography;
using ProblemsBoardLib.Models;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows;

namespace ProblemsBoardLib
{
    /// <summary>
    /// Вспомогательный статический класс для упрощения работы с данными
    /// </summary>
    public enum Roles
    {
        RAdmin = 1,
        RResponsible = 2,
        RHeaderWorker = 3
    } 

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
                try
                {
                    var value = prop.GetValue(source);
                    dprop.SetValue(destination, value);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

		/// <summary>
		/// Шифрует строку алгоритмом SHA256
		/// </summary>
		/// <param name="str">Шифруемая строка</param>
		/// <returns>Зашифровання строка</returns>
		public static string EncryptString(string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("Argument can't be null", nameof(str));

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

        public static List<Theme> ThemesSerialization(string path)
        {
            List<Theme> themes = new();

            string jsonstring = File.ReadAllText(path, Encoding.UTF8);

            themes = JsonSerializer.Deserialize<List<Theme>>(jsonstring);

            return themes;
        }

        /// <summary>
        /// Класс генератора логина, содержит <see cref="Adjectives"/> и <see cref="Nouns"/> для генерации и метод генерации
        /// </summary>
        class LoginGenerator
        {
            [JsonPropertyName("Adjectives")]
            public List<string> Adjectives { get; private set; } = new();

            [JsonPropertyName("Nouns")]
            public List<string> Nouns { get; private set; } = new();

            internal string GenerateLogin()
            {

                Random rand = new Random();

                string login = "";
                login += Adjectives[rand.Next(0, Adjectives.Count)];
                login += Nouns[rand.Next(0, Nouns.Count)];

                return login;
            }
            public void LoadJsonFile(string jsonFilePath)
            {
                if (!File.Exists(jsonFilePath))
                {
                    throw new FileNotFoundException($"JSON file not found: {jsonFilePath}");
                }

                string jsonString = File.ReadAllText(jsonFilePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                try
                {
                    var data = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonString, options);
                    if (data != null)
                    {
                        Adjectives = data.GetValueOrDefault("Adjectives", new List<string>());
                        Nouns = data.GetValueOrDefault("Nouns", new List<string>());
                    }
                }
                catch (JsonException ex)
                {
                    throw new JsonException("Error deserializing JSON file.", ex);
                }
            }
        }

        /// <summary>
        /// Метод генерации логина, использующий классы <see cref="LoginGenerator"/> и
        /// <seealso cref="JsonSerializer"/> для получения <see cref="LoginGenerator.Adjectives"/> и <see cref="LoginGenerator.Nouns"/>
        /// </summary>
        /// <returns>Строка логина</returns>
        public static string GenerateLogin()
        {
            LoginGenerator loginGenerator = new();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "LoginGenerator.json");

            try
            {
                loginGenerator.LoadJsonFile(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return loginGenerator.GenerateLogin();
        }

        /// <summary>
        /// Метод генерации пароля
        /// </summary>
        /// <param name="lenght">Длина пароля</param>
        /// <returns>Строка пароля</returns>
        public static string GeneratePassword(int lenght)
        {
            string lchars = "abcdefghijklmnopqrstuvwxyz";
            string uchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string nums = "0123456789";
            string spec = "!@#$%&*";
            string password = new string('#', lenght);
            char[] pass_chars = password.ToCharArray();
            Random random = new();
            Random charrand = new();

            for (int i = 0; i < password.Length; i++)
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        pass_chars[i] = lchars[charrand.Next(0, lchars.Length)];
                        break;
                    case 1:
                        pass_chars[i] = uchars[charrand.Next(0, uchars.Length)];
                        break;
                    case 2:
                        pass_chars[i] = nums[charrand.Next(0, nums.Length)];
                        break;
                    case 3:
                        pass_chars[i] = spec[charrand.Next(0, spec.Length)];
                        break;
                }
            }

            password = "";
            foreach (char c in pass_chars)
            {
                password += c;
            }

            return password;
        }

        public static void CopyToClipboard(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Text cannot be null or empty", nameof(text));
            }

            try
            {
                Clipboard.SetText(text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
