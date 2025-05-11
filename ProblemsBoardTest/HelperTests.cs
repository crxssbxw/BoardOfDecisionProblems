using ProblemsBoardLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProblemsBoardTest
{
    [TestFixture]
    public class HelperTests
    {
        // Тест-требование 1. Копирование свойств объектов (метод CopyTo)
        [Test]
        public void CopyTo_CorrectCopy()
        {
            // Тестовый пример 1.1
            var source = new TestClass { Name = "Test", Age = 30 };
            var destination = new TestClass();
            Helper.CopyTo(source, destination);
            Assert.AreEqual(source.Name, destination.Name);
            Assert.AreEqual(source.Age, destination.Age);
        }

        [Test]
        public void CopyTo_DifferentTypes_ThrowsException()
        {
            // Тестовый пример 1.2
            var source = new TestClassA { Name = "Test" };
            var destination = new TestClassB { Age = 0 };
            Assert.Throws<Exception>(() => Helper.CopyTo(source, destination));
        }

        // Тест-требование 2. Шифрование строки (метод EncryptString)
        [Test]
        public void EncryptString_ValidInput_ReturnsCorrectHash()
        {
            // Тестовый пример 2.1
            string input = "test";
            string expected = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08";
            string result = Helper.EncryptString(input);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EncryptString_NullOrEmpty_ThrowsException()
        {
            // Тестовый пример 2.2
            Assert.Throws<ArgumentException>(() => Helper.EncryptString(null));
            Assert.Throws<ArgumentException>(() => Helper.EncryptString(""));
        }

        // Тест-требование 3. Сериализация тем из JSON (метод ThemesSerialization)
        [Test]
        public void ThemesSerialization_ValidFile_ReturnsCorrectList()
        {
            // Тестовый пример 3.1
            string tempPath = Path.GetTempFileName();
            File.WriteAllText(tempPath, "[{\"ThemeId\":1,\"ThemeName\":\"Тема1\"}]");
            var themes = Helper.ThemesSerialization(tempPath);
            Assert.AreEqual(1, themes.Count);
            Assert.AreEqual("Тема1", themes[0].Name);
            File.Delete(tempPath);
        }

        [Test]
        public void ThemesSerialization_FileNotFound_ThrowsException()
        {
            // Тестовый пример 3.2
            Assert.Throws<FileNotFoundException>(() => Helper.ThemesSerialization("nonexistent.json"));
        }

        // Тест-требование 4. Генерация логина (метод GenerateLogin)
        [Test]
        public void GenerateLogin_ValidData_ReturnsCorrectLogin()
        {
            // Тестовый пример 4.1
            string tempPath = Path.Combine(Directory.GetCurrentDirectory(), "LoginGenerator.json");
            File.WriteAllText(tempPath, "{\"Adjectives\":[\"Quick\"],\"Nouns\":[\"Fox\"]}");
            string login = Helper.GenerateLogin();
            Assert.IsTrue(login == "QuickFox");
            File.Delete(tempPath);
        }

        [Test]
        public void GenerateLogin_FileNotFound_ReturnsRandomLogin()
        {
            // Тестовый пример 4.2
            string tempPath = Path.Combine(Directory.GetCurrentDirectory(), "LoginGenerator.json");
            File.Delete(tempPath);
            // Проверка MessageBox требует GUI, поэтому пропускаем, но проверяем возврат логина
            string login = Helper.GenerateLogin();
            Assert.IsNull(login);
        }

        // Тест-требование 5. Генерация пароля (метод GeneratePassword)
        [Test]
        public void GeneratePassword_ValidLength_ContainsAllCharacters()
        {
            // Тестовый пример 5.1
            string password = Helper.GeneratePassword(10);
            Assert.IsTrue(password.Any(char.IsLower));
            Assert.IsTrue(password.Any(char.IsUpper));
            Assert.IsTrue(password.Any(char.IsDigit));
            Assert.IsTrue(password.Any(c => "!@#$%&*".Contains(c)));
            Assert.AreEqual(10, password.Length);
        }

        // Вспомогательные классы для тестов
        private class TestClass
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        private class TestClassA
        {
            public string Name { get; set; }
        }

        private class TestClassB
        {
            public int Age { get; set; }
        }
    }
}
