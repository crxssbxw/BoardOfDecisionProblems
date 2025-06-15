using ProblemsBoardLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProblemsBoardLib.Tools
{
    public static class LoggingTool
    {
        private static DatabaseContext dbContext { get; set; } = new();
        private static string message { get; set; }

        private static void SaveLogToFile(LogEvent logEvent)
        {
            using (StreamWriter sw = new("log.txt", true, Encoding.UTF8))
            {
                sw.WriteLine($"{logEvent.Date}:{logEvent.Time} | {message}");
            }
        }

        public static void NewAdminAuthorizationData(Department department)
        {
            message = $"Установлены новые данные администратора участка:\n" +
                $"(ID:{department.DepartmentId}) Название: {department.Name}; Номер: {department.ViewerNumber}";
            LogEvent logEvent = new LogEvent()
            {
                Comment = message,
                Title = "Новые данные администратора",
                //Object = $"{nameof(department.Admin)}",
                Table = $"{nameof(Department)}",
                User = "Admin",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now)
            };

            SaveLogToFile(logEvent);
        }
        public static void NewAdminAuthorizationData(Department department, string login = "", string password = "")
        {
            message = $"Установлены новые данные администратора участка:\n" +
                $"(ID:{department.DepartmentId}) Название: {department.Name}; Номер: {department.ViewerNumber}";

            if (!string.IsNullOrEmpty(login))
                message += $"\nНовый логин: {login}";
            if (!string.IsNullOrEmpty(password))
                message += $"\nХэш-код пароля: {Helper.EncryptString(password)}" ;

            LogEvent logEvent = new LogEvent()
            {
                Comment = message,
                Title = "Новые данные администратора",
                //Object = $"{nameof(department.Admin)}",
                Table = $"{nameof(Department)}",
                User = "Admin",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now)
            };

            SaveLogToFile(logEvent);
        }

        public static void NewResponsibleSet(Department department/*, Responsible responsible*/)
        {
            message = $"На участке {department.Name} {department.ViewerNumber} (ID:{department.DepartmentId}) назначен новый ответственный ";
            LogEvent logEvent = new LogEvent()
            {
                Comment = message,
                Title = "Новый ответственный",
                //Object = $"{nameof(Department.Responsibles)}",
                //Table = $"{nameof(Department)} {nameof(Responsible)}",
                User = "Admin",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now)
            };

            SaveLogToFile(logEvent);
        }

        public static void ResponsibleReset(Department department, Worker prev, Worker current)
        {
            message = $"На участке {department.Name} {department.ViewerNumber} (ID:{department.DepartmentId}) " +
                $"переназначен ответственный:\n{prev?.WorkerInfo} ===> {current.WorkerInfo}";
            LogEvent logEvent = new LogEvent()
            {
                Comment = message,
                Title = "Переназначен ответственный",
                Object = $"{nameof(Department.Workers)}",
                Table = $"{nameof(Department)} {nameof(Worker)}",
                User = "Admin",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now)
            };

            SaveLogToFile(logEvent);
        }
        public static void HeaderReset(Department department, Worker? prev, Worker current)
        {
            message = $"На участке {department.Name} {department.ViewerNumber} (ID:{department.DepartmentId}) " +
                $"переназначен главный по сброу проблем:\n{prev?.WorkerInfo} ===> {current.WorkerInfo}";
            LogEvent logEvent = new LogEvent()
            {
                Comment = message,
                Title = "Переназначен главный по сбору проблем",
                Object = $"{nameof(Department.Workers)}",
                Table = $"{nameof(Department)} {nameof(Worker)}",
                User = "Admin",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now)
            };

            SaveLogToFile(logEvent);
        }

        public static void BoardSet(Department department)
        {
            message = $"Доска для участка {department.Name} {department.ViewerNumber} (ID:{department.DepartmentId}) была успешно настроена!";
            LogEvent logEvent = new LogEvent()
            {
                Comment = message,
                Title = "Доска настроена",
                Object = $"{nameof(Department)}",
                Table = $"{nameof(Department)}",
                User = "Admin",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now)
            };

            SaveLogToFile(logEvent);
        }

        public static void NewThemeSet(Theme theme)
        {
            message = $"Была добавлена новая тема:\n" +
                $"(ID:{theme.ThemeId}) Название: {theme.Name}; Описание: {theme.Description}";
            //if (theme.Department != null)
            //    message += $"; Участок {theme.Department}";

            LogEvent logEvent = new LogEvent()
            {
                Comment = message,
                Title = "Добавлена тема",
                Object = $"{nameof(Theme)}",
                Table = $"{nameof(Theme)}",
                User = "BoardUser",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now)
            };

            SaveLogToFile(logEvent);
        }

        public static void ThemeChanged(Theme theme)
        {
            message = $"Была изменена тема:\n" +
                $"(ID:{theme.ThemeId}) Название: {theme.Name}; Описание: {theme.Description}";
            //if (theme.Department != null)
            //    message += $"; Участок {theme.Department}";

            LogEvent logEvent = new LogEvent()
            {
                Comment = message,
                Title = "Изменена тема",
                Object = $"{nameof(Theme)}",
                Table = $"{nameof(Theme)}",
                User = "BoardUser",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now)
            };

            SaveLogToFile(logEvent);
        }

        public static void NewProblemAdded(Problem problem)
        {
            message = $"На участке {problem.Department} была создана новая проблема.\n" +
                $"(ID:{problem.ProblemId}) Тема: {problem.ThemeName}, Ответственный: ";

            LogEvent logEvent = new LogEvent()
            {
                Comment = message,
                Title = "Создана проблема",
                Object = $"{nameof(Problem)}",
                Table = $"{nameof(Problem)}",
                User = "BoardUser",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now)
            };

            SaveLogToFile(logEvent);
        }

        public static void ProblemDecided(Problem problem)
        {
            message = $"На участке {problem.Department} была решена проблема.\n" +
                $"(ID:{problem.ProblemId}) Тема: {problem.ThemeName}, Ответственный: \n" +
                $"Дата создания: {problem.DateOccurance}; Дата решения: {problem.DateElimination}; Решение: {problem.Decision}";

            LogEvent logEvent = new LogEvent()
            {
                Comment = message,
                Title = "Решена проблема",
                Object = $"{nameof(Problem)}",
                Table = $"{nameof(Problem)}",
                User = $"{problem.Responsible.WorkerInfo}",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now)
            };

            SaveLogToFile(logEvent);
        }

        public static void ReportCreated(Report report)
        {
            message = $"Был создан отчет:\n" +
                $"(ID:{report.ReportId}) Тип: {report.FullType}; Порядковый номер: {report.Number}";

            LogEvent logEvent = new LogEvent()
            {
                Comment = message,
                Title = "Создан отчет",
                Object = $"{nameof(Report)}",
                Table = $"{nameof(Report)}",
                User = "BoardUser",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now)
            };

            SaveLogToFile(logEvent);
        }

        public static void ReportSaved(string path, string type)
        {
            message = $"Был сохранен отчет {type}\n\tПуть: {path}";

            LogEvent logEvent = new LogEvent()
            {
                Comment = message,
                Title = "Сохранен отчет",
                Object = $"{nameof(Report)}",
                Table = $"{nameof(Report)}",
                User = "BoardUser",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now)
            };

            SaveLogToFile(logEvent);
        }
    }
}
