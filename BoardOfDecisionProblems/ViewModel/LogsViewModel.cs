using BoardOfDecisionProblems.Commands;
using BoardOfDecisionProblems.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoardOfDecisionProblems.ViewModel
{
    public class LogsViewModel : BaseViewModel
    {
        private ObservableCollection<Log> _logs = new();
        private ObservableCollection<LogEvent> _logEvents = new();

        public ObservableCollection<Log> Logs
        {
            get => _logs;
            set
            {
                _logs = value;
                OnPropertyChanged(nameof(Logs));
            }
        }

        public ObservableCollection<LogEvent> LogEvents
        {
            get => _logEvents;
            set
            {
                _logEvents = value;
                OnPropertyChanged(nameof(LogEvents));
            }
        }

        private LogEvent _selectedLogEvent = new();
        public LogEvent SelectedLogEvent
        {
            get => _selectedLogEvent;
            set
            {
                _selectedLogEvent = value;
                OnPropertyChanged(nameof(SelectedLogEvent));
            }
        }

        private void SaveLogMethod()
        {
            LogEvent logEvent = new LogEvent()
            {
                Title = "Сохранение Лога",
                User = "Admin",
                Table = "LogEvents"
            };


            string Path = "";

            // Открытие диалогового окна для сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "Log";
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.Filter = "Text documents (.txt)|*.txt";


            if (saveFileDialog.ShowDialog() == true)
            {
                Path = saveFileDialog.FileName;
            }
            else return;

            logEvent.Comment = "Путь: " + Path;
            dbContext.Add(logEvent);
            LogEvents.Add(logEvent);
            dbContext.SaveChanges();

            // Запись лога в файл
            using (StreamWriter sw = new StreamWriter(Path))
            {
                foreach(LogEvent logevent in LogEvents)
                {
                    sw.Write($"### {logevent.Date} - {logevent.Time} : {logevent.Title}");
                    if (logevent.Object != null) sw.Write($" [Объект {logevent.Object}]");
                    if (logevent.Table != null) sw.Write($" [Таблица {logevent.Table}]");
                    if (logevent.Comment != null) sw.Write($"\n\t\t \"{logevent.Comment}\"");
                    sw.Write($" -<{logevent.User}>- ###\n");
                }
            }

            // Сохранение файла лога в БД
            byte[] LogData;
            using (FileStream fs = new(Path, FileMode.Open))
            {
                LogData = new byte[fs.Length];
                fs.Read(LogData, 0, LogData.Length);
            }
            Log log = new Log()
            {
                DateTime = DateTime.Now,
                LogFile = LogData
            };
            dbContext.Add(log);
            Logs.Add(log);
            dbContext.SaveChanges();

            var openFolder = MessageBox.Show("Открыть папку с файлом?", "Внимание", MessageBoxButton.YesNo);

            if(openFolder == MessageBoxResult.Yes)
            {
                string argument = "/select, \"" + Path + "\"";
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
        }

        private RelayCommand _saveLog;
        public RelayCommand SaveLog
        {
            get
            {
                return _saveLog ?? (_saveLog = new(obj =>
                {
                    SaveLogMethod();
                }, obj => true));
            }
        }
    }
}
